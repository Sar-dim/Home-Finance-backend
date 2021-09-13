using Common.Models;
using Common.Models.Exceptions;
using Domain.Entity;
using Infractructure.Services.Implimintations;
using Infractructure.Services.Interfaces;
using Moq;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace UnitTests
{
    public class SourceServiceTest
    {
        private ISourceService _service;
        private Mock<IBaseRepository<OperationSource>> _sourceRepoMock = new Mock<IBaseRepository<OperationSource>>();

        public SourceServiceTest()
        {
            _service = new SourceService(_sourceRepoMock.Object);
        }

        [Fact]
        public void Add_Test()
        {
            //Arrange
            var sourceModel = new SourceModel { Id = Guid.NewGuid(), Name = "Expanse", Type = (int)OperationType.Expanse };
            OperationSource result = null;
            _sourceRepoMock.Setup(x => x.Create(It.IsAny<OperationSource>()))
                .Returns((OperationSource os) => result = os);

            //Act
            _service.AddSource(sourceModel);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, sourceModel.Name);
            Assert.Equal((int)result.Type, sourceModel.Type);
            Assert.Throws<BadRequestException>(() => _service.AddSource(null));
        }

        [Fact]
        public void Get_Test()
        {
            //Arrange
            var source = new OperationSource("Expanse", OperationType.Expanse);
            _sourceRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<OperationSource, bool>>>()))
                .Returns(new List<OperationSource> { source }.AsQueryable);

            //Act
            var result1 = _service.GetSource(source.Name);
            var result2 = _service.GetSource(source.Id);

            //Assert
            Assert.Equal(source.Name, result1.Name);
            Assert.Equal(source.Name, result2.Name);
            Assert.Equal((int)source.Type, result1.Type);
            Assert.Equal((int)source.Type, result2.Type);
            Assert.Throws<BadRequestException>(() => _service.GetSource(null));
        }

        [Fact]
        public void Update_Test()
        {
            //Arrange
            var expected = new SourceModel { Id = Guid.NewGuid(), Name = "Expanse", Type = (int)OperationType.Expanse };
            var expectedSource = new OperationSource(expected.Name, OperationType.Expanse);
            expectedSource.Id = expected.Id;
            var actual = new SourceModel { Id = Guid.NewGuid(), Name = "Income", Type = (int)OperationType.Income };
            _sourceRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<OperationSource, bool>>>()))
                .Returns(new List<OperationSource> { expectedSource }.AsQueryable);
            _sourceRepoMock.Setup(x => x.Update(It.IsAny<OperationSource>()))
                .Returns((OperationSource os) =>
                {
                    actual.Id = os.Id;
                    actual.Name = os.Name;
                    actual.Type = (int)os.Type;
                    return os;
                });

            //Act
            _service.UpdateSource(expected);

            //Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Throws<BadRequestException>(() => _service.UpdateSource(null));
        }

        [Fact]
        public void Delete_Test()
        {
            //Arrange
            var Id = Guid.NewGuid();
            bool deleted = false;
            _sourceRepoMock.Setup(x => x.Delete(Id)).Callback(() => deleted = true);
            _sourceRepoMock.Setup(x => x.GetFiltered(It.IsAny<Expression<Func<OperationSource, bool>>>()))
                .Returns(new List<OperationSource> { new OperationSource("Expanse", OperationType.Expanse) }.AsQueryable);

            //Act
            _service.DeleteSource(Id);

            //Assert
            Assert.True(deleted);
            Assert.Throws<BadRequestException>(() => _service.DeleteSource(Guid.Empty));
        }
    }
}
