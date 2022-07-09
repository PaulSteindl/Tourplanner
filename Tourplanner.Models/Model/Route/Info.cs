using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Models
{
    public class Info
    {
        public int Statuscode { get; set; }
        public List<string?> Messages { get; set; }
    }
}
