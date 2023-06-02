using DAL;
using NetTopologySuite.Operation.Distance;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace BL
{
    public class TourLogHandler
    {
        public List<tourlogs> Search(int tourid)
        {
            DALConfiguration configuration = new DALConfiguration();
            TourLogContext context = new TourLogContext(configuration.configuration);
            TourLogSQLRepository tourLogSQLRepository = new TourLogSQLRepository(context);

            List<tourlogs> list = new List<tourlogs>();

            var tourlogrepo = tourLogSQLRepository.GetAllTourLogsFromTour(tourid);
            foreach (var log in tourlogrepo)
            {
                list.Add(log);
            }

            int count = 0;
            float time = 0;
            float distance = 0;
            float rating = 0;
            foreach (tourlogs tour in list)
            {

                time += Convert.ToSingle(tour.time, CultureInfo.InvariantCulture);
                distance += Convert.ToSingle(tour.distance, CultureInfo.InvariantCulture);
                rating += Convert.ToSingle(tour.rating, CultureInfo.InvariantCulture);

                count++;
            }
            time = time / count;
            distance = distance / count;
            rating = rating / count;

            tourlogs tourlog = new tourlogs();
            tourlog.id = 0;
            tourlog.tourid = tourid;
            tourlog.time = time.ToString();
            tourlog.distance = distance.ToString();
            tourlog.rating = rating.ToString();
            tourlog.comment = "average values of all tourlogs";


            list.Insert(0, tourlog);

            return list;
        }
        
        public void DeleteTourLogForDB(int id)
        {
            DALConfiguration configuration = new DALConfiguration();

            TourLogContext context = new TourLogContext(configuration.configuration);
            TourLogSQLRepository repository = new TourLogSQLRepository(context);
            repository.Remove(id);
        }
        public void EditTourLogForDB(int id, int tourid, string time, string distance, string rating, string comment)
        {
            float timecheck;
            bool timecheckfloat = float.TryParse(time, out timecheck);
            float distancecheck;
            bool distancecheckfloat = float.TryParse(distance, out distancecheck);
            int ratingcheck;
            bool ratingcheckint = int.TryParse(rating, out ratingcheck);

            if (timecheckfloat && distancecheckfloat && ratingcheckint && ratingcheck >= 0 && ratingcheck <= 9)
            {
                tourlogs log = new tourlogs();
                log.id = id;
                log.tourid = tourid;
                log.time = time;
                log.distance = distance;
                log.rating = rating;
                log.comment = comment;
                DALConfiguration configuration = new DALConfiguration();

                TourLogContext context = new TourLogContext(configuration.configuration);
                TourLogSQLRepository repository = new TourLogSQLRepository(context);
                repository.Update(log);
            }
        }
        
        public List<tourlogs> SearchForLogs(string searchtext)
        {
            DALConfiguration configuration = new DALConfiguration();
            TourLogContext context = new TourLogContext(configuration.configuration);
            TourLogSQLRepository repository = new TourLogSQLRepository(context);

            List<tourlogs> tourlogs = new List<tourlogs>();
            var tourrepo = repository.SearchForTourLogs(searchtext);

            foreach (var log in tourrepo)
            {
                tourlogs.Add(log);
            }
            return tourlogs;
        }
        public void CreateTourLogForDB(int _tourid, string _time, string _distance, string _rating, string _comment)
        {
            DALConfiguration configuration = new DALConfiguration();
            TourLogContext tourLogContext = new TourLogContext(configuration.configuration);
            TourLogSQLRepository tourLogSQLRepository = new TourLogSQLRepository(tourLogContext);
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
                tourLogSQLRepository.Add(tourlog);
            }
            else
            {
                throw new Exception("rating no integer or integer too big or small");
            }
        }
    }
}
