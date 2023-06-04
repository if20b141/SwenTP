using DAL;
using BL;
using UIL;
using System.Collections.Generic;
using Telerik.JustMock;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Telerik.JustMock.Helpers;
using NetTopologySuite.Operation.Distance;
using Npgsql.Internal.TypeHandlers.GeometricHandlers;

namespace OurUnitTests
{
    public class MockTourLogHandler : ITourLogHandler
    {
        public List<tourlogs> dblogs = new List<tourlogs>();
        public List<tourlogs> Search(int tourid)
        {
            List<tourlogs> list = dblogs.FindAll(x => x.tourid == tourid);
            return list;
        }
        public List<tourlogs> SearchForLogs(string searchtext)
        {
            List<tourlogs> list = dblogs.FindAll(t => t.time.Contains(searchtext) || t.distance.Contains(searchtext) || t.rating.Contains(searchtext) || t.comment.Contains(searchtext));
            return list;
        }
        public int DeleteTourLogForDB(int id)
        {
            tourlogs logs = dblogs.Find(t => t.id == id);
            if (logs != null)
            {
                dblogs.Remove(logs);

                return 1;
            }
            else
            {
                return 0;
            }

        }
        public int EditTourLogForDB(int id, int tourid, string time, string distance, string rating, string comment)
        {
            float timecheck;
            bool timecheckfloat = float.TryParse(time, out timecheck);
            float distancecheck;
            bool distancecheckfloat = float.TryParse(distance, out distancecheck);
            int ratingcheck;
            bool ratingcheckint = int.TryParse(rating, out ratingcheck);

            if (timecheckfloat && distancecheckfloat && ratingcheckint && ratingcheck >= 0 && ratingcheck <= 9)
            {
                tourlogs logtofind = dblogs.Find(t => t.id == id);
                tourlogs log = new tourlogs { id = id, time = time, distance = distance, rating = rating, comment = comment };
                dblogs.Remove(logtofind);
                dblogs.Add(log);

                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int CreateTourLogForDB(int _tourid, string _time, string _distance, string _rating, string _comment)
        {
            int check;
            bool checkint = int.TryParse(_rating, out check);
            if (checkint == true && check <= 9 && check >= 0)
            {
                tourlogs tourlog = new tourlogs();
                tourlog.tourid = _tourid;
                tourlog.time = _time;
                tourlog.distance = _distance;
                tourlog.rating = _rating;
                tourlog.comment = _comment;
                dblogs.Add(tourlog);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public class MockTourHandler
    {
        public List<tours> tourlist = new List<tours>();
        public string EditTourForDB(int _Id, string _Tourname, string _Description, string _From, string _To, string _Type) {
            tours tourtofind = tourlist.Find(t => t.id == _Id);
            if (tourtofind != null)
            {
                tours tour = new tours();
                tour.tourname = _Tourname;
                tour.description = _Description;
                tour.startpoint = _From;
                tour.endpoint = _To;
                tourlist.Remove(tourtofind);
                tourlist.Add(tour);
                return "ok";
            }
            else
            {
                return "noresult";
            }
        }
        public List<tours> SearchForTours(string searchtext) {
            List<tours> list= tourlist.FindAll(t => t.tourname.Contains(searchtext) || t.description.Contains(searchtext) || t.startpoint.Contains(searchtext) || t.endpoint.Contains(searchtext) || t.type.Contains(searchtext) || t.distance.Contains(searchtext) || t.time.Contains(searchtext));
            return list;
        }
        public int SearchForID(string tourname) {
            tours tour = tourlist.Find(t => t.tourname == tourname);
            if(tour != null)
            {
                return tour.id;
            }
            else 
            {
                return 0; 
            }
        }
        public int DeleteTourForDB(int id) {
            tours tour = tourlist.Find(t => t.id == id);
            if(tour != null)
            {
                tourlist.Remove(tour);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public tours CreateTourForDB(string _Tourname, string _Description, string _From, string _To, string _Type) {
            tours tour = new tours { tourname = _Tourname, description = _Description, startpoint = _From, endpoint = _To, type = _Type};
            if (tourlist.Find(t => t.tourname == _Tourname) == null)
            {
                tourlist.Add(tour);
                return tour;
            }
            else
            {
                tours emptytour = new tours();
                return emptytour;
            }
            
        }
        public List<tours> GetAllTours() {
            return tourlist;
        }
    }
    public class UnitTest1
    {
        
        [Fact]
        public void CreateTourLogTrue()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();
            
            

            int response = logHandler.CreateTourLogForDB(1, "1", "1", "5", "test");

            Xunit.Assert.Equal(response, 1);



        }
        [Fact]
        public void CreateTourLogFalse()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();
            int response = logHandler.CreateTourLogForDB(1, "1", "1", "5.5", "test");

            Xunit.Assert.Equal(response, 0);
        }
        [Fact]
        public void SearchForLogsByIDTrue()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.Search(1);

            Xunit.Assert.Equal(list, logs);

        }
        [Fact]
        public void SearchForLogsByIDFalse()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.Search(5);

            Xunit.Assert.Equal(0, list.Count);
        }
        [Fact]
        public void EditTourLogTrue()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.Search(1);
            int result = logHandler.EditTourLogForDB(list.First().id, 1, "2", "2", "5", "test 2");

            Xunit.Assert.Equal(1, result);
        }
        [Fact]
        public void EditTourLogFalse()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.Search(1);
            int result = logHandler.EditTourLogForDB(list.First().id, 1, "2", "2", "5.5", "test 2");

            Xunit.Assert.Equal(0, result);
        }
        [Fact]
        public void SearchTourLogsTrue()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.SearchForLogs("test");
            Xunit.Assert.Equal(logs, list);
        }
        [Fact]
        public void SearchTourLogsFalse()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;
            List<tourlogs> list = logHandler.SearchForLogs("swen");
            Xunit.Assert.Equal(0, list.Count);
        }
        [Fact]
        public void SearchForTourIDTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours {id= 1, tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };

            handler.tourlist.Add(tour);
            int response = handler.SearchForID("test");

            Xunit.Assert.Equal(response, 1);
        }
        [Fact]
        public void SearchForTourIDFalse()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };

            handler.tourlist.Add(tour);
            int response = handler.SearchForID("swen");

            Xunit.Assert.Equal(response, 0);
        }
        [Fact]
        public void SerchForToursTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);


            handler.tourlist.Add(tour);
            List<tours> response = handler.SearchForTours("test");

            Xunit.Assert.Equal(list, response);
        }
        [Fact]
        public void SerchForToursFalse()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);


            handler.tourlist.Add(tour);
            List<tours> response = handler.SearchForTours("swen");

            List<tours> emptylist = new List<tours>();

            Xunit.Assert.Equal(emptylist, response);
        }
        [Fact]
        public void EditTourTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);


            handler.tourlist.Add(tour);
            int id = handler.SearchForID("test");
            string response = handler.EditTourForDB(id, "swen", "swen", "swen", "swen", "swen");

            Xunit.Assert.Equal("ok", response);
        }
        [Fact]
        public void EditTourFalse()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);


            handler.tourlist.Add(tour);
            int id = handler.SearchForID("test");
            string response = handler.EditTourForDB(27, "swen", "swen", "swen", "swen", "swen");

            Xunit.Assert.Equal("noresult", response);
        }
        [Fact]
        public void GetAllToursTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);

            handler.tourlist.Add(tour);
            List<tours> response = handler.GetAllTours();

            Xunit.Assert.Equal(list, response);
        }
        [Fact]
        public void GetAllToursFalse()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);

            
            List<tours> response = handler.GetAllTours();

            List<tours> check = new List<tours>();

            Xunit.Assert.Equal(check, response);
        }
        [Fact]
        public void DeleteTourTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);

            handler.tourlist.Add(tour);

            int id = handler.SearchForID("test");
            int response = handler.DeleteTourForDB(id);

            Xunit.Assert.Equal(1, response);
        }
        [Fact]
        public void DelteTourFalse()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours {id = 1, tourname = "test", description = "test", startpoint = "test", endpoint = "test", distance = "test", time = "test", type = "test", information = "test" };
            List<tours> list = new List<tours>();
            list.Add(tour);

            handler.tourlist.Add(tour);

            
            int response = handler.DeleteTourForDB(27);

            Xunit.Assert.Equal(0, response);
        }
        [Fact]
        public void DeleteLogTrue()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs {id =1, distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;

            int response = logHandler.DeleteTourLogForDB(1);

            Xunit.Assert.Equal(1, response);
        }
        [Fact]
        public void DeleteLogFalse()
        {
            MockTourLogHandler logHandler = new MockTourLogHandler();

            List<tourlogs> logs = new List<tourlogs>();
            tourlogs log = new tourlogs { distance = "1", rating = "5", time = "1", comment = "test", tourid = 1 };
            logs.Add(log);

            logHandler.dblogs = logs;

            int response = logHandler.DeleteTourLogForDB(27);

            Xunit.Assert.Equal(0, response);
        }
        [Fact]
        public void CreateTourTrue()
        {
            MockTourHandler handler = new MockTourHandler();
            tours tour = new tours { tourname = "test", description = "test", startpoint = "test", endpoint = "test",  type = "test"};

            tours response = handler.CreateTourForDB("test", "test", "test", "test", "test");
            Xunit.Assert.Equal(tour.tourname, response.tourname);
        }
        



    }
}