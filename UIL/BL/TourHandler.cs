using DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class TourHandler
    {
        public string Tourname;
        public string Description;
        public string From;
        public string To;
        public string Type;
        public int Id;
        public void EditTourForDB(int _Id, string _Tourname, string _Description, string _From, string _To, string _Type)
        {
            Id = _Id;
            Tourname = _Tourname;
            Description = _Description;
            From = _From;
            To = _To;
            Type = _Type;

            EditMapQuest();
        }
        
        public List<tours> SearchForTours(string searchtext)
        {
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);

            List<tours> tours = new List<tours>();
            var tourrepo = repository.SearchForTours(searchtext);




            foreach (var tour in tourrepo)
            {
                tours.Add(tour);
            }
            return tours;
        }
        public int SearchForID(string tourname)
        {
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            
            var tourrepo = repository.SelectTour(tourname).ToList();
            foreach(var tour in tourrepo)
            {
                return tour.id;
            }
            return 0;
        }
        public void DeleteTourForDB(int id)
        {
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);

            List<tourlogs> logs = new List<tourlogs>();
            TourLogHandler logHandler = new TourLogHandler();
            logs = logHandler.Search(id);
            foreach(tourlogs log in logs)
            {
                logHandler.DeleteTourLogForDB(log.id);
            }

            repository.Remove(id);
        }
        public async Task<tours> CreateTourForDB(string _Tourname, string _Description, string _From, string _To, string _Type)
        {
            Tourname = _Tourname;
            Description = _Description;
            From = _From;
            To = _To;
            Type = _Type;

            tours tour = await AddMapQuest();
            return tour;
           
        }
        public List<tours> GetAllTours()
        {
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            List<tours> list = new List<tours>();
            list = repository.GetAllTours().ToList();
            return list;
        }
        public async Task<tours> AddMapQuest()
        {
            MapServer server = new BL.MapServer();
            string response = await MapServer.MapRequest(From, To, Type);
            string time = server.TourImageRequest(From, To, Tourname, Type);
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

            tour.information = Tourname + time + ".png";
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            repository.Add(tour);
            return tour;

        }
        public async void EditMapQuest()
        {
            MapServer server = new BL.MapServer();
            string response = await MapServer.MapRequest(From, To, Type);
            string time = server.TourImageRequest(From, To, Tourname, Type);
            JObject jsondata = JObject.Parse(response);
            var route = jsondata["route"];
            var Distance = route["distance"].Value<string>();
            var Time = route["formattedTime"].Value<string>();

            tours tour = new tours();
            tour.id = Id;
            tour.tourname = Tourname;
            tour.description = Description;
            tour.startpoint = From;
            tour.endpoint = To;
            tour.type = Type;
            tour.distance = Distance;
            tour.time = Time;
            tour.information = Tourname + time + ".png";
            DALConfiguration configuration = new DALConfiguration();

            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            repository.Update(tour);
        }

    }
}
