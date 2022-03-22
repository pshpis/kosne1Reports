using System;

namespace Server.ReportsExceptions.Specific
{
    public class NotEnoughRightsException : ReportsGlobalException
    {
        public NotEnoughRightsException() : base()
        {
        }

        public NotEnoughRightsException(string message) : base(message)
        {
        }

        public NotEnoughRightsException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}