using Microsoft.VisualBasic;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MapServer
    {


        public static async Task<string> MapRequest(string _From, string _To, string _Type)
        {
            string APIKey = "kOaSvJyHVCfDUCek157MirgXXDZLRs0R";
            string HTTPCommand;
            HTTPCommand = "https://www.mapquestapi.com/directions/v2/route?key=" + APIKey + "&from=" + _From +"&to=" + _To + "&routeType=" + _Type + "&unit=k";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(HTTPCommand);

            if (response.IsSuccessStatusCode)
            {
                string responsecontent = await response.Content.ReadAsStringAsync();
                return responsecontent;
            }
            else
            {
                throw new Exception("Keine Daten von MapQuest" + response.StatusCode);

            }
        }
        public string TourImageRequest(string _From, string _To, string tourname, string type)
        {
            string APIKey = "kOaSvJyHVCfDUCek157MirgXXDZLRs0R";
            string HTTPCommand;
            HTTPCommand = "https://www.mapquestapi.com/staticmap/v5/map?start=" + _From + "&end=" + _To + "&size=600,400@2x&key=" + APIKey;
            Uri uri = new Uri(HTTPCommand);
            
            WebClient client = new WebClient();
            //FileStream fs = File.Create(tourname + ".png");
            string time =DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            
            client.DownloadFile(uri, AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" + tourname + time + ".png");

            return time;

        }
        
    }
}
