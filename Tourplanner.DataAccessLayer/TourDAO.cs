using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using System.Drawing;

namespace Tourplanner.DataAccessLayer
{
    internal class TourDAO : ITourDAO
    {
        public Task<Tour> AddTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public Task<Tour> ModifyTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public Task<Tour> DeleteTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public string SaveImage(byte[] mapArray, string routeId)
        {
            //Check for file
            string fullPath = $"{Path.GetDirectoryName(Path.GetFullPath($".\\{routeId}.jpeg"))}";
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                var fs = File.Create($"./{fullPath}");
                fs.Write(mapArray);
                fs.Close();
            }

            return fullPath;
        }
    }
}
