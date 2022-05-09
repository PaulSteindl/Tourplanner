using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSub.Search
{
    internal class StandardSearchEngine : ISearchEngine
    {
        public IEnumerable<string> SearchFor(string? searchText)
        {
            if  (string.IsNullOrEmpty(searchText))
            {
                return Enumerable.Empty<string>();
            }
            return new[] { $"Search Result 1 for {searchText}", $"Search Result 2 for {searchText}", $"Search Result 3 for {searchText}" };
        }
    }
}
