using System;

namespace Server.ReportsExceptions.Specific
{
    public class WrongIdException : ReportsGlobalException
    {
        public WrongIdException() : base()
        {
        }

        public WrongIdException(string message) : base(message)
        {
        }

        public WrongIdException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}