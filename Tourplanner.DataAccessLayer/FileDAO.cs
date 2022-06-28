using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.DataAccessLayer
{
    internal class FileDAO
    {
        public void SaveImage(byte[] mapArray, string fullPath)
        {
            Directory.CreateDirectory(fullPath);
            var fs = File.Create($"./{fullPath}");
            fs.Write(mapArray);
            fs.Close();
        }
    }
}
