using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer.TourManager
{
    public interface ITourManager
    {
        Task Create(Tour tour);
        Task Modify(Tour tour);
        Task Delete(Tour tour);

        // searchTour ...
    }
}
