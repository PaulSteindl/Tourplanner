using Newtonsoft.Json;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class ImportManager : IImportManager
    {
        private IFileDAO fileDAO;
        private ITourManager tourManager;
        private readonly ILogger logger = Shared.LogManager.GetLogger<ImportManager>();

        public ImportManager(IFileDAO fileDAO, ITourManager tourManager)
        {
            this.fileDAO = fileDAO;
            this.tourManager = tourManager;
        }

        public async Task<Tour> ImportTour(string filepath)
        {
            var jsonString = fileDAO.ReadImportFile(filepath);

            var importTour = JsonConvert.DeserializeObject<Tour>(jsonString);

            if(importTour != null)
                importTour = await tourManager.NewTour(importTour.Name, importTour.Description ?? String.Empty, importTour.From, importTour.To, importTour.Transporttype);

            logger.Debug($"Tour imported with id: [{importTour.Id}]");

            return importTour ?? throw new NullReferenceException("Error with tour import");
        }
    }
}
