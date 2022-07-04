using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.DataAccessLayer
{
    public interface IPostgreSqlDAOConfiguration
    {
        public string ConnectionString { get; }
    }
}
