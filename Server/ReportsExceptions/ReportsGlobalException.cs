using System;

namespace Server.ReportsExceptions
{
    public class ReportsGlobalException : Exception
    {
        public ReportsGlobalException() : base()
        {
        }

        public ReportsGlobalException(string message) : base(message)
        {
        }

        public ReportsGlobalException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}