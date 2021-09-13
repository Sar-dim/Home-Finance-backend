using System.Security.Claims;

namespace net_core_backend.Services.Interfaces
{
    public interface IPersonService
    {
        public void AddPerson();
        public ClaimsIdentity GetIdentity(string username, string password);
    }
}
