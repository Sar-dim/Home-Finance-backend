using System.Net;

namespace Common.Models.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound)
        {
        }
    }
}
