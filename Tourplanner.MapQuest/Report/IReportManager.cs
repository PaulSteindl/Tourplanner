using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer.Report
{
    public interface IReportManager
    {
        public bool CreateTourReport(Tour tour, string path);
        public bool CreateSummarizeReport(string path);
    }
}
