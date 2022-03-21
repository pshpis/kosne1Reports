using System;

namespace DAL.ModelsExceptions
{
    public class CreatingException : ArgumentNullException
    {
        public CreatingException() : base()
        {
        }

        public CreatingException(string message) : base(message)
        {
        }

        public CreatingException(string nameof, string message) : base(nameof, message)
        {
        }
        
        
    }
}