using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Tourplanner.Models;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Tourplanner.DataAccessLayer
{
    public class FileDAO : IFileDAO
    {
        public string SaveImage(byte[] mapArray, string routeId)
        {
            Directory.CreateDirectory("C:\\TourImages");
            var path = $"C:\\TourImages\\{routeId}.png";
            var fs = File.Create(path);
            fs.Write(mapArray);
            fs.Close();

            return path;
        }

        public string ReadImportFile(string filepath)
        {
            string json = File.ReadAllText(filepath);

            return json;
        }

        public void SaveExportTour(string jsonString, string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string filename = $"\\TourExport_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FF")}.json";

            path += "\\Exported_Tours";

            Directory.CreateDirectory(path);
            File.WriteAllText(path + filename, jsonString);
        }

        public void CreateReport(Tour tour, string fullpath)
        {
            PdfWriter writer = new PdfWriter(fullpath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph tourHeader = new Paragraph(tour.Name)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY);
            document.Add(tourHeader);

            document.Add(new Paragraph(tour.Description)
                            .SetFontSize(10)
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                        );

            Paragraph listHeader = new Paragraph("Details")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY);

            List detaillist = new List()
                    .SetSymbolIndent(12)
                    .SetFontSize(10)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
            detaillist.Add(new ListItem("From: " + tour.From))
                    .Add(new ListItem("To: " + tour.To))
                    .Add(new ListItem("Transport Type: " + tour.Transporttype.ToString()))
                    .Add(new ListItem("Distance: " + String.Format("{0:0.##}", tour.Distance) + "km"))
                    .Add(new ListItem("Estimated Time: " + CalcTimeFormated(tour.Time)))
                    .Add(new ListItem("Popularity: " + tour.Popularity.ToString()))
                    .Add(new ListItem("Is Childfriendly: " + tour.ChildFriendly.ToString()));
            document.Add(listHeader);
            document.Add(detaillist);
            document.Add(new Paragraph());

            Paragraph mapHeader = new Paragraph("Map")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY);
            document.Add(mapHeader);
            ImageData imageData = ImageDataFactory.Create(tour.PicPath);
            document.Add(new Image(imageData));

            document.Add(new AreaBreak());

            Paragraph logHeader = new Paragraph("Logs")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY);
            document.Add(logHeader);

            foreach (var log in tour.Logs)
            {
                List loglist = new List()
                    .SetSymbolIndent(12)
                    .SetFontSize(10)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));

                loglist.Add(new ListItem("Date: " + log.Date.ToString()))
                    .Add(new ListItem("Difficulty: " + log.Difficulty.ToString()))
                    .Add(new ListItem("Time Required: " + CalcTimeFormated(log.TotalTime)))
                    .Add(new ListItem("Rating: " + log.Rating.ToString()))
                    .Add(new ListItem("Comment: "));
                document.Add(loglist);

                document.Add(new Paragraph(log.Comment)
                            .SetFontSize(10)
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                        );

                document.Add(new Paragraph());
            }

            document.Close();
        }

        private string CalcTimeFormated(int time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            string answer = "00h:00m:00s";

            //sec in einem Tag
            if (time > 86399)
            {
                answer = string.Format("{0:D1}d:{1:D2}h:{2:D2}m:{3:D2}s",
                                t.Days,
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
            }
            else
            {
                answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
            }

            return answer;
        }
    }
}
