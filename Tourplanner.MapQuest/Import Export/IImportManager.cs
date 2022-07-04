using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.BusinessLayer
{
    public interface IImportManager
    {
        public void ImportTour(string filepath);
    }
}
