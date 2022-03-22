using System;

namespace Server.ReportsExceptions.Specific
{
    public class ForbiddenActionException : ReportsGlobalException
    {
        public ForbiddenActionException() : base()
        {
        }

        public ForbiddenActionException(string message) : base(message)
        {
        }

        public ForbiddenActionException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}