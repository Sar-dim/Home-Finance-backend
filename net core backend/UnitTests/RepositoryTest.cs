using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Database;
using net_core_backend.Models;
using net_core_backend.Repositories.Implimintations;
using net_core_backend.Repositories.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class RepositoryTest
    {
        public DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Test").Options;
        public IBaseRepository<OperationSource> _repository;
        public ApplicationContext _context;

        public RepositoryTest()
        {
            _context = new ApplicationContext(options, new HttpContextAccessor());
            _repository = new BaseRepository<OperationSource>(_context);
        }

        [Fact]
        public void Create_Test()
        {
            //Arrange
            var model = new OperationSource("Expanse", OperationType.Expanse);

            //Act
            _repository.Create(model);

            //Assert
            Assert.Equal(_context.OperationSource.FirstOrDefault(x => x.Id == model.Id), model);
            Assert.Throws<ArgumentNullException>(() => _repository.Create(null));
        }

        [Fact]
        public void Delete_Test()
        {
            //Arrange
            var model1 = new OperationSource("Income1", OperationType.Income);
            var model2 = new OperationSource("Income2", OperationType.Income);
            var model3 = new OperationSource("Income3", OperationType.Income);

            //Act
            _context.OperationSource.Add(model1);
            _context.OperationSource.Add(model2);
            _context.OperationSource.Add(model3);
            _context.SaveChanges();
            _repository.Delete(model2.Id);

            //Assert
            Assert.Contains(model1, _context.OperationSource);
            Assert.DoesNotContain(model2, _context.OperationSource);
            Assert.Contains(model3, _context.OperationSource);
            Assert.Throws<ArgumentNullException>(() => _repository.Delete(Guid.Empty));
        }

        [Fact]
        public void Get_Test()
        {
            //Arrange
            var model = new OperationSource("Expanse", OperationType.Expanse);

            //Act
            _context.OperationSource.Add(model);
            _context.SaveChanges();
            var resultGetAll = _repository.GetAll();
            var resultGet = _repository.Get(model.Id);
            var resultGetFiltered = _repository.GetFiltered(x => x.Id == model.Id).FirstOrDefault();

            //Assert
            Assert.NotNull(resultGetAll);
            Assert.Contains(model, resultGetAll);
            Assert.Equal(resultGet, model);
            Assert.Equal(resultGetFiltered, model);
            Assert.Throws<ArgumentNullException>(() => _repository.Get(Guid.Empty));
            Assert.Throws<ArgumentNullException>(() => _repository.GetFiltered(null));
        }

        [Fact]
        public void Update_Test()
        {
            //Arrange
            var model1 = new OperationSource("Expanse", OperationType.Expanse);
            var model2 = new OperationSource("Income", OperationType.Income)
            {
                Id = model1.Id,
            };

            _context.OperationSource.Add(model1);
            _context.SaveChanges();

            //Act
            _repository.Update(model2);

            //Assert
            Assert.Equal(_context.OperationSource.FirstOrDefault(x => x.Id == model1.Id).Name, model2.Name);
            Assert.Equal(_context.OperationSource.FirstOrDefault(x => x.Id == model1.Id).Type, model2.Type);
            Assert.Throws<ArgumentNullException>(() => _repository.Update(null));
            Assert.Throws<ArgumentNullException>(() => _repository.Update(new OperationSource("Income", OperationType.Expanse)));
        }
    }
}
