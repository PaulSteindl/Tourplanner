﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.DataAccessLayer
{
    public interface IFileDAO
    {
        string SaveImage(byte[] mapArray, string fullPath);
    }
}