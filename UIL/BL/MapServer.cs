using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MapServer
    {


        public static async Task<string> MapRequest()
        {
            string APIKey = "kOaSvJyHVCfDUCek157MirgXXDZLRs0R";
            string HTTPCommand;
            HTTPCommand = "https://www.mapquestapi.com/directions/v2/route?key=" + APIKey + "&from=Clarendon Blvd,Arlington,VA&to=2400+S+Glebe+Rd,+Arlington,+VA";
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
    }
}
