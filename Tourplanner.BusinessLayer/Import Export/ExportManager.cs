using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using Newtonsoft.Json;
using System.Text.Json;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class ExportManager : IExportManager
    {
        private ITourDAO tourDAO;
        private IFileDAO fileDAO;
        private readonly ILogger logger = LogingManager.GetLogger<ExportManager>();

        public ExportManager(ITourDAO tourDAO, IFileDAO fileDAO)
        {
            this.tourDAO = tourDAO;
            this.fileDAO = fileDAO;
        }

        //gibt einen bool zurück ob erfolgreich oder nicht
        public bool ExportTourById(Guid tourId, string path)
        {
            try
            {
                Tour exportTour = tourDAO.SelectTourById(tourId);

                if (exportTour != null)
                {
                    exportTour.Id = Guid.Empty;

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var jsonString = System.Text.Json.JsonSerializer.Serialize(exportTour, options);

                    fileDAO.SaveExportTour(jsonString, path);

                    logger.Debug($"Exported Tour: [{exportTour.Name}] ");

                    return true;
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Couldnt export Tour: [{ex.Message}]");
                return false;
            }

            logger.Debug($"Couldnt export Tour: exportTour is null ");

            return false;
        }
    }
}
