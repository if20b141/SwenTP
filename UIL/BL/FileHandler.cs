using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql.Internal.TypeHandlers.GeometricHandlers;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace BL
{
    public class FileHandler
    {
        public void SaveToFile(tours tour)
        {
            if (tour != null)
            {
                TourLogHandler logHandler = new TourLogHandler();
                List<tourlogs> list = new List<tourlogs>();
                list = logHandler.Search(tour.id);
                string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + tour.tourname + time + ".txt";
                FileStream fs = File.Create(path);
                list.RemoveAt(0);
                tour.logs = list;
                

                string text = JsonSerializer.Serialize(tour);
                Byte[] bytes = Encoding.UTF8.GetBytes(text);
                
                fs.Write(bytes, 0, bytes.Length);

                    
                
                


                //File.WriteAllText(path, text);
            }
            else
            {
                throw new Exception("no tour selected!");
            }
            
        }
        public async void ReadFromFile(string filename)
        {
            if (filename != null)
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + filename + ".txt");
                string text = sr.ReadToEnd();
                tours tour = JsonSerializer.Deserialize<tours>(text);
                
                //List<tourlogs> list = JsonSerializer.Deserialize<List<tourlogs>>(text);

                TourHandler handler = new TourHandler();
                TourLogHandler logHandler = new TourLogHandler();

                tours thetour =  await handler.CreateTourForDB(tour.tourname, tour.description, tour.startpoint, tour.endpoint, tour.type);
                
                ReadLogsFromFile(tour.tourname, tour.logs);
            }
            else
            {
                throw new Exception("no input!");
            }
        }
        public void ReadLogsFromFile(string tourname, List<tourlogs> logs)
        {
            TourHandler handler = new TourHandler();
            TourLogHandler logHandler = new TourLogHandler();
            int id = handler.SearchForID(tourname);
            if (id != 0)
            {
                foreach (tourlogs log in logs)
                {
                    logHandler.CreateTourLogForDB(id, log.time, log.distance, log.rating, log.comment);
                }
            }
            else
            {
                throw new Exception("nix gut");
            }
        }
        public void TourReport(tours tour)
        {
            string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + tour.tourname + "Report" + time + ".pdf";
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics graphics = XGraphics.FromPdfPage(page);
            string fontname = "Calibri";
            XFont font = new XFont(fontname, 12, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));

            string towrite = "";
            TourHandler handler = new TourHandler();
            TourLogHandler logHandler = new TourLogHandler();
            List<tours> tourlist = new List<tours>();
            tourlist = handler.GetAllTours();
            int height = 0;

            towrite = "tourid: " + tour.id + " ";
            towrite = towrite + "tourname: " + tour.tourname;
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 20;
            XImage image = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" + tour.information);
            graphics.DrawImage(image, 0, height, 200, 200);
            height = height + 200;
            towrite = "description: " + tour.description;
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 20;
            towrite = "startpoint: " + tour.startpoint + " endpoint: " + tour.endpoint;
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 20;
            towrite = "Transportation Type: " + tour.type;
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 20;
            towrite = "Distance: " + tour.distance + " Time: " + tour.time;
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 40;
            towrite = "Tourlogs: ";
            graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
            height = height + 20;

            pdf.Save(path);
            page = pdf.AddPage();
            graphics = XGraphics.FromPdfPage(page);
            height = 0;

            List<tourlogs> logs = new List<tourlogs>();
            logs = logHandler.Search(tour.id);
            
            foreach (tourlogs log in logs)
            {
                towrite = "Tourlognumber: " + log.id;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                towrite = "Distance: " + log.distance + " Time: " + log.time + " Rating: " + log.rating;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                towrite = "Comment: " + log.comment;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 40;

                pdf.Save(path);
                page = pdf.AddPage();
                graphics = XGraphics.FromPdfPage(page);
                height = 0;
            }
        }
        public void CompleteReport()
        {
            string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\" + "CompleteReport" + time + ".pdf";
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics graphics = XGraphics.FromPdfPage(page);
            string fontname = "Calibri";
            XFont font = new XFont(fontname, 12, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.WinAnsi));

            string towrite = "";
            TourHandler handler = new TourHandler();
            TourLogHandler logHandler = new TourLogHandler();
            List<tours> tourlist = new List<tours>();
            tourlist = handler.GetAllTours();
            int height = 0;
            

            foreach (tours tour in tourlist)
            {
                towrite = "tourid: " + tour.id + " ";
                towrite = towrite + "tourname: " + tour.tourname;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                XImage image = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" + tour.information);
                graphics.DrawImage(image,0,height,200,200);
                height = height + 200;
                towrite = "description: " + tour.description;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                towrite = "startpoint: " + tour.startpoint + " endpoint: " + tour.endpoint;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                towrite = "Transportation Type: " + tour.type;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;
                towrite = "Distance: " + tour.distance + " Time: " + tour.time;
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 40;
                towrite = "Tourlogs: ";
                graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                height = height + 20;

                pdf.Save(path);
                page = pdf.AddPage();
                graphics = XGraphics.FromPdfPage(page);
                height = 0;

                List<tourlogs> logs = new List<tourlogs>();
                logs = logHandler.Search(tour.id);
                
                foreach(tourlogs log in logs)
                {
                    towrite = "Tourlognumber: " + log.id;
                    graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                    height = height + 20;
                    towrite = "Distance: " + log.distance + " Time: " + log.time + " Rating: " + log.rating;
                    graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                    height = height + 20;
                    towrite = "Comment: " + log.comment;
                    graphics.DrawString(towrite, font, XBrushes.Black, new XRect(0, height, page.Width, page.Height), XStringFormats.TopLeft);
                    height = height + 40;
                    
                    pdf.Save(path);
                    page = pdf.AddPage();
                    graphics = XGraphics.FromPdfPage(page);
                    height = 0;
                }

            }
            //pdf.Save(path);

            
            
            
        }
        
    }
}
