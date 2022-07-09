using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface IFileDAO
    {
        public string SaveImage(byte[] mapArray, Guid tourId);
        public string ReadImportFile(string filepath);
        public void SaveExportTour(string jsonString, string path);
        public void DeleteImage(Guid tourId);
        public void DeleteFile(string path);
    }
}