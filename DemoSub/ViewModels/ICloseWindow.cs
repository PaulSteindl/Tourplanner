using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.ViewModels
{
    internal interface ICloseWindow
    {
        public Action? Close { get; set; }
    }
}
