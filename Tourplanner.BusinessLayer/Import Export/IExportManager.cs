using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.BusinessLayer
{
    public interface IExportManager
    {
        public bool ExportTourById(Guid tourId, string path);
    }
}
