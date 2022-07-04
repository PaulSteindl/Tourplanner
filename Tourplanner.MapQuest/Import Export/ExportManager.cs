using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using Newtonsoft.Json;
using System.Text.Json;

namespace Tourplanner.BusinessLayer.Import_Export
{
    public class ExportManager : IExportManager
    {
        private ITourDAO tourDAO;
        private IFileDAO fileDAO;

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

                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return false;
        }
    }
}
