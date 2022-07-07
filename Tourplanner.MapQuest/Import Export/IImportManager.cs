using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface IImportManager
    {
        public Task<Tour> ImportTour(string filepath);
    }
}
