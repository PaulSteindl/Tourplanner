using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tourplanner.BusinessLayer
{
    public class ReportManager : IReportManager
    {
        ITourDAO tourDAO;
        ICalculateAttributes calcA;

        public ReportManager(ITourDAO tourDAO, ICalculateAttributes calcA)
        {
            this.tourDAO = tourDAO;
            this.calcA = calcA;
        }

        public bool CreateTourReport(Tour tour, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullpath = path + tour.Name + "_Report.pdf";

            if (!File.Exists(fullpath) && tour != null)
            {
                try
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
                            .Add(new ListItem("Estimated Time: " + calcA.CalcTimeFormated(tour.Time)))
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
                            .Add(new ListItem("Time Required: " + calcA.CalcTimeFormated(log.TotalTime)))
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

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            
            return false;
        }

        public bool CreateSummarizeReport(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullpath = path + "SummarizeReport.pdf";

            var tours = tourDAO.SelectAllTours();

            if (!File.Exists(fullpath) && tours != null && tours.Count > 0)
            {
                try
                {
                    PdfWriter writer = new PdfWriter(fullpath);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    Paragraph tourHeader = new Paragraph("Summarize Report")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                            .SetFontSize(16)
                            .SetBold()
                            .SetFontColor(ColorConstants.DARK_GRAY);
                    document.Add(tourHeader);

                    Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
                    table.AddHeaderCell(getHeaderCell("Tourname"));
                    table.AddHeaderCell(getHeaderCell("Rating"));
                    table.AddHeaderCell(getHeaderCell("Time"));
                    table.SetFontSize(10).SetBackgroundColor(ColorConstants.WHITE);

                    foreach (var t in tours)
                    {
                        table.AddCell(t.Name);
                        table.AddCell(Enum.Parse<PopularityEnum>(calcA.AverageRatingCalc(t.Logs).ToString()).ToString());
                        table.AddCell(calcA.CalcTimeFormated(Convert.ToInt32(calcA.AverageTimeCalc(t.Logs))));
                    }

                    document.Add(table);

                    document.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            
            return false;
        }

        static private Cell getHeaderCell(String s)
        {
            return new Cell().Add(new Paragraph(s)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
