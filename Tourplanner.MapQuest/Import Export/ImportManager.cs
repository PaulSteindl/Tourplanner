using Newtonsoft.Json;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public class ImportManager : IImportManager
    {
        private IFileDAO fileDAO;
        private ITourManager tourManager;

        public ImportManager(IFileDAO fileDAO, ITourManager tourManager)
        {
            this.fileDAO = fileDAO;
            this.tourManager = tourManager;
        }

        public void ImportTour(string filepath)
        {
            var jsonString = fileDAO.ReadImportFile(filepath);

            var importTour = JsonConvert.DeserializeObject<Tour>(jsonString);

            if(importTour != null)
                tourManager.newTour(importTour.Name, importTour.Description ?? String.Empty, importTour.From, importTour.To, importTour.Transporttype);
        }
    }
}
