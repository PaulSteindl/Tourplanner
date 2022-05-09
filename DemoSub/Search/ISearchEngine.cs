using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSub.Search
{
    internal interface ISearchEngine
    {
        IEnumerable<string> SearchFor(string? searchText);
    }
}
