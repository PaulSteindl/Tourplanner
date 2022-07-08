using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Tourplanner.Models;
using Tourplanner.Shared;

namespace Tourplanner.DataAccessLayer
{
    public class FileDAO : IFileDAO
    {
        private readonly ILogger logger = LogingManager.GetLogger<FileDAO>();
        readonly string picDirPath = "..\\..\\..\\..\\TourImages";

        public string SaveImage(byte[] mapArray, Guid tourId)
        {
            Directory.CreateDirectory(picDirPath);
            var path = $"{picDirPath}\\{tourId}.png";
            var fs = File.Create(path);
            fs.Write(mapArray);
            fs.Close();

            logger.Debug($"Created picture [{path}].");

            return Path.GetFullPath(path);
        }

        public void DeleteImage(Guid tourId)
        {
            var path = $"{picDirPath}\\{tourId}.png";

            if (File.Exists(path))
                File.Delete(path);

            logger.Debug($"Deleted picture [{path}].");
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

            logger.Debug($"Created Export Tour [{path + filename}].");
        }
    }
}
