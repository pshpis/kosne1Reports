using System;

namespace Server.ReportsExceptions
{
    public class ReportException : Exception
    {
        public ReportException() : base()
        {
        }

        public ReportException(string message) : base(message)
        {
        }

        public ReportException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}