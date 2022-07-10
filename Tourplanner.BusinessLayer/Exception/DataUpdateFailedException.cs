using System;

namespace Tourplanner.BusinessLayer
{
    public class DataUpdateFailedException : Exception
    {
        public DataUpdateFailedException()
        {
        }

        public DataUpdateFailedException(string message) : base(message)
        {
        }

        public DataUpdateFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}