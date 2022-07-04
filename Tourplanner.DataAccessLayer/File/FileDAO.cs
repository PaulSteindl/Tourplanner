using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    internal class FileDAO
    {
        public string SaveImage(byte[] mapArray, string routeId)
        {
            Directory.CreateDirectory("C:\\TourImages");
            var path = $"C:\\TourImages\\{routeId}";
            var fs = File.Create(path);
            fs.Write(mapArray);
            fs.Close();

            return path;
        }

        public string ReadImportFile(string filepath)
        {
            string json = File.ReadAllText(filepath);

            return json;
        }

        public void SaveExportTour(string jsonString, string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string filename = $"\\TourExport_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FF")}.json";

            path += "\\Exported_Tours";

            Directory.CreateDirectory(path);
            File.WriteAllText(path + filename, jsonString);
        }
    }
}
