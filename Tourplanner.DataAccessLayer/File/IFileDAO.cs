﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface IFileDAO
    {
        public string SaveImage(byte[] mapArray, string routeId);
        public string ReadImportFile(string filepath);
        public void SaveExportTour(string jsonString, string path);
    }
}