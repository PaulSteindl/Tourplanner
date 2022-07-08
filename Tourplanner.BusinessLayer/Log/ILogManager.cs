using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourLogManager
    {
        public Log? CreateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Guid tourId);
        public Task<Log?> UpdateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Log log);
        public void DeleteLog(Guid logId);
        public IEnumerable<Log> GetAllLogsByTourId(Guid tourId);
    }
}