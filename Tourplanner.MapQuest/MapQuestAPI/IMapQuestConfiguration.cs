using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.BusinessLayer
{
    public interface IMapQuestConfiguration
    {
        public string DirectionUrl { get; }

        public string MapUrl { get;}

        public string MapQuestKey {get;}
    }
}
