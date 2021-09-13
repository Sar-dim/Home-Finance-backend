using System;

namespace Common.Models.Exceptions
{
    public class BaseException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public BaseException(string message, int statudCode)
        {
            Message = message;
            StatusCode = statudCode;
        }
    }
}
