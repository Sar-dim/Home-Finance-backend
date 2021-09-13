using Common.Models.Exceptions;
using Moq;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Implimintations;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace UnitTests
{
    public class ClaimsDummy : ClaimsIdentity
    {
        public ClaimsDummy(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(claims, authenticationType, nameType, roleType)
        {
        }

        public override bool Equals(Object obj)
        {
            var item = obj as ClaimsDummy;

            if (item == null)
            {
                return false;
            }

            if (Claims.FirstOrDefault(x => x.Type == "PersonId").Value ==
                item.Claims.FirstOrDefault(x => x.Type == "PersonId").Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class PersonServiceTest
    {
        private IPersonService _service;
        private Mock<IBaseRepository<Person>> _personRepoMock = new Mock<IBaseRepository<Person>>();

        public PersonServiceTest()
        {
            _service = new PersonService(_personRepoMock.Object);
        }

        [Fact]
        public void GetIdentity_Test()
        {
            //Arrange
            var person = new Person { Id = Guid.NewGuid(), Login = "login", Password = "password", Role = "admin" };
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim("PersonId", person.Id.ToString())
                };
            ClaimsDummy expected =
            new ClaimsDummy(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            _personRepoMock.Setup(x => x.GetAll()).Returns(new List<Person>() { person });

            //Act
            var result = _service.GetIdentity(person.Login, person.Password);

            //Assert
            expected.Equals(result);
            Assert.Throws<BadRequestException>(() => _service.GetIdentity(null, null));
            Assert.Throws<BadRequestException>(() => _service.GetIdentity("", ""));
            Assert.Throws<NotFoundException>(() => _service.GetIdentity("user", "user"));
        }
    }
}
