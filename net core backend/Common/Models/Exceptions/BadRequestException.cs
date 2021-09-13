using System.Net;

namespace Common.Models.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}
