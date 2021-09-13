using Common.Models.Exceptions;
using net_core_backend.Models;
using net_core_backend.Repositories.Interfaces;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace net_core_backend.Services.Implimintations
{
    public class PersonService : IPersonService
    {
        private readonly IBaseRepository<Person> _person;

        public PersonService(IBaseRepository<Person> person)
        {
            this._person = person ?? throw new ArgumentNullException(nameof(person));
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            if (username == string.Empty || username == null || password == string.Empty || password == null)
            {
                throw new BadRequestException("Login or password was empty");
            }
            List<Person> persons = _person.GetAll();
            Person person = persons.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim("PersonId", person.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            else
            {
                // если пользователя не найдено
                throw new NotFoundException("Person not found");
            }
        }
        public void AddPerson()
        {
            _person.Create(new Person
            {
                Id = Guid.NewGuid(),
                Login = "login",
                Password = "password",
                Role = "user"
            });

        }
    }
}
