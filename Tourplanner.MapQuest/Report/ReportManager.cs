using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;

namespace Tourplanner.BusinessLayer.Report
{
    public class ReportManager : IReportManager
    {
        IFileDAO fileDAO;

        public ReportManager(IFileDAO fileDAO)
        {
            this.fileDAO = fileDAO;
        }

        public bool CreateReport(Tour tour, string path)
        {
            var fullpath = path + tour.Name + "_Report.pdf";

            if (!File.Exists(fullpath) && tour != null)
            {
                try
                {
                    fileDAO.CreateReport(tour, fullpath);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;

            return true;
        }
    }
}
