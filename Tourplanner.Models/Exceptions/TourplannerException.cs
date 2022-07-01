using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Exceptions
{
    public abstract class TourplannerException : Exception
    {
        public TourplannerException()
        {

        }

        public TourplannerException(string message) : base(message)
        {
        
        }

        public TourplannerException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
