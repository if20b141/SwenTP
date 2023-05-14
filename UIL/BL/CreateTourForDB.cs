using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;
using DAL;
using Microsoft.Extensions.Configuration;

namespace BL
{
    public class CreateTourForDB
    {
        public string Tourname;
        public string Description;
        public string From;
        public string To;
        public string Type;
        public CreateTourForDB(string _Tourname, string _Description, string _From, string _To, string _Type)
        {
            Tourname = _Tourname;
            Description = _Description;
            From = _From;
            To = _To;
            Type = _Type;

            MapQuestTest();
        }
        public async void MapQuestTest()
        {
            MapServer server = new BL.MapServer();
            string response = await MapServer.MapRequest(From, To, Type);
            JObject jsondata = JObject.Parse(response);
            var route = jsondata["route"];
            var Distance = route["distance"].Value<string>();
            var Time = route["formattedTime"].Value<string>();

            tours tour = new tours();
            tour.tourname = Tourname;
            tour.description = Description;
            tour.startpoint = From;
            tour.endpoint = To;
            tour.type = Type;
            tour.distance = Distance;
            tour.time = Time;
            tour.information = "hallo";
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            repository.Add(tour);
        }

    }
}
