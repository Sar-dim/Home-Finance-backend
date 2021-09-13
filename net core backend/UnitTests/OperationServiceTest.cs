using Common.Models.Exceptions;
using Domain.Entity;
using Moq;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Implimintations;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;

namespace UnitTests
{
    public class OperationServiceTest
    {
        private IOperationService _service;
        private Mock<IBaseRepository<Operation>> _operationRepoMock = new Mock<IBaseRepository<Operation>>();
        private Mock<IBaseRepository<Person>> _personRepoMock = new Mock<IBaseRepository<Person>>();
        private Mock<IBaseRepository<OperationSource>> _sourceRepoMock = new Mock<IBaseRepository<OperationSource>>();

        public OperationServiceTest()
        {
            _service = new OperationService(_operationRepoMock.Object, _personRepoMock.Object, _sourceRepoMock.Object);
        }

        [Fact]
        public void GetOperationForTime_Test()
        {
            // Arrange
            IQueryable<Operation> operations = GetFakeOperations().AsQueryable();
            IQueryable<Operation> nullOperations = new List<Operation>().AsQueryable();
            _operationRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<Operation, bool>>>()))
                .Returns((Expression<Func<Operation, bool>> filter) => operations.Where(filter));

            // Act
            var result = _service.GetOperationForTime(new OperationRequest
            {
                DateFirst = DateTime.Now
            });


            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Operations);
            Assert.Contains(operations, x => x.Id == result.Operations.First().Id);
            Assert.Throws<BadRequestException>(() => _service.GetOperationForTime(null));
        }

        [Fact]
        public void Add_Test()
        {
            // Arrange
            Operation actual = null;
            Operation expected = new Operation
            {
                Amount = 152,
                Id = Guid.NewGuid(),
                OperationDateTime = DateTime.Now,
                PersonId = Guid.NewGuid(),
                Source = new OperationSource("Expanse", OperationType.Expanse)
            };
            expected.SourceId = expected.Source.Id;

            _operationRepoMock.Setup(x => x.Create(It.IsAny<Operation>()))
                .Returns((Operation op) =>
                {
                    actual = op;
                    return op;
                });
            _sourceRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<OperationSource, bool>>>()))
                .Returns(new List<OperationSource>() { expected.Source }.AsQueryable);
            var operationRequest = new OperationModel(expected.Id, expected.OperationDateTime, (int)expected.Source.Type,
                expected.Source.Name, expected.Amount, expected.PersonId, "admin");
            var cl = new ClaimsIdentity(new Claim[] { new Claim("PersonId", expected.PersonId.ToString()) });

            // Act
            _service.AddOperation(operationRequest, cl);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(actual.OperationDateTime.Date, expected.OperationDateTime.Date);
            Assert.Equal(actual.Amount, expected.Amount);
            Assert.Equal(actual.SourceId, expected.SourceId);
            Assert.Equal(actual.PersonId, expected.PersonId);
            Assert.Throws<BadRequestException>(() => _service.AddOperation(null, cl));
            Assert.Throws<NotFoundException>(() => _service.AddOperation(operationRequest, null));
        }

        [Fact]
        public void Update_Test()
        {
            // Arrange
            Operation actual = new Operation
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                OperationDateTime = DateTime.Now.AddDays(-1),
                PersonId = Guid.NewGuid(),
                Source = new OperationSource("Expanse", OperationType.Expanse)
            };
            actual.SourceId = actual.Source.Id;
            Operation expected = new Operation
            {
                Id = Guid.NewGuid(),
                Amount = 200,
                OperationDateTime = DateTime.Now,
                PersonId = Guid.NewGuid(),
                Source = new OperationSource("Income", OperationType.Income)
            };
            expected.SourceId = expected.Source.Id;
            var cl = new ClaimsIdentity(new Claim[] { new Claim("PersonId", expected.PersonId.ToString()) });
            var operationRequest = new OperationModel(expected.Id, expected.OperationDateTime, (int)expected.Source.Type,
                expected.Source.Name, expected.Amount, expected.PersonId, "admin");
            _operationRepoMock.Setup(x => x.Update(It.IsAny<Operation>()))
                .Returns((Operation op) =>
                {
                    actual = op;
                    return op;
                });
            _operationRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<Operation, bool>>>()))
                .Returns(new List<Operation>() { expected }.AsQueryable);

            //Act
            _service.UpdateOperation(operationRequest, cl);

            //Assert
            Assert.Equal(actual, expected);
            Assert.Throws<BadRequestException>(() => _service.UpdateOperation(null, cl));
            Assert.Throws<NotFoundException>(() => _service.UpdateOperation(operationRequest, null));
        }

        [Fact]
        public void Delete_Test()
        {
            //Arrange
            var operationId = Guid.NewGuid();
            bool deleted = false;
            var cl = new ClaimsIdentity(new Claim[] { new Claim("PersonId", Guid.NewGuid().ToString()) });
            _operationRepoMock.Setup(x => x.Delete(operationId)).Callback(() => deleted = true);
            _operationRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<Operation, bool>>>()))
                .Returns(new List<Operation>() { new Operation() }.AsQueryable);

            //Act
            _service.DeleteOperation(operationId, cl);

            //Assert
            Assert.True(deleted);
            Assert.Throws<BadRequestException>(() => _service.DeleteOperation(Guid.Empty, cl));
            Assert.Throws<NotFoundException>(() => _service.DeleteOperation(operationId, null));
        }

        private IEnumerable<Operation> GetFakeOperations()
        {
            var operations = new List<Operation>();
            var fakePerson = new Person
            {
                Id = Guid.NewGuid()
            };
            for (int i = -10; i < 10; i++)
            {
                var source = GetRandomSource(i);
                var operation = new Operation
                {
                    Id = Guid.NewGuid(),
                    Amount = new Random().NextDouble() * 100,
                    OperationDateTime = DateTime.Now.AddDays(i),
                    Person = fakePerson,
                    PersonId = fakePerson.Id,
                    Source = source,
                    SourceId = source.Id
                };

                operations.Add(operation);
            }

            return operations;
        }

        private OperationSource GetRandomSource(int num)
        {
            if (num % 2 == 0)
            {
                return new OperationSource($"Some name {num}", OperationType.Income);
            }

            return new OperationSource($"Some name {num}", OperationType.Expanse);
        }
    }
}