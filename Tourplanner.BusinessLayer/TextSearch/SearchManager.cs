using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class SearchManager : ISearchManager
    {

        private readonly ILogger logger = Shared.LogManager.GetLogger<SearchManager>();

        public IEnumerable<Tour> FindMatchingTours(List<Tour> tours, string? searchText = null)
        {
            logger.Debug($"Searching in Tour/Log for [{searchText}]");

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return tours;
            }

            return tours.Where(t =>
                t.Name.ToLower().Contains(searchText.ToLower()) ||
                t.Transporttype.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.Description.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.From.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.To.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.Distance.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.Time.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.Popularity.ToString().ToLower().Contains(searchText.ToLower()) ||
                t.Logs.Where(l =>
                    l.Comment.ToLower().Contains(searchText.ToLower()) ||
                    l.Date.ToString().ToLower().Contains(searchText.ToLower()) ||
                    l.Difficulty.ToString().ToLower().Contains(searchText.ToLower()) ||
                    l.TotalTime.ToString().ToLower().Contains(searchText.ToLower()) ||
                    l.Rating.ToString().ToLower().Contains(searchText.ToLower())
                ).Count() > 0
            );
        }
    }
}
