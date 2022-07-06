using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface IReportManager
    {
        public bool CreateReport(Tour tour, string path);
    }
}
