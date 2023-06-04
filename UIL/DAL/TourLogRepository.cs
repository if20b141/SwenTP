using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TourLogNOSQLRepository : ITourLogRepository
    {
        public ICollection<tourlogs> GetAllTourLogs()
        {
            throw new System.NotImplementedException();
        }
        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }
        public tourlogs Add(tourlogs tourlog)
        {
            throw new System.NotImplementedException();
        }
        public tourlogs Update(tourlogs tourlog)
        {
            throw new System.NotImplementedException();
        }

    }
    public class TourLogSQLRepository : ITourLogRepository
    {
        private readonly TourLogContext context;
        public TourLogSQLRepository(TourLogContext context)
        {
            this.context = context;
        }
        public tourlogs Add(tourlogs tourlog)
        {
            context.Add(tourlog);
            context.SaveChanges();

            return tourlog;
        }
        public ICollection<tourlogs> GetAllTourLogs()
        {
            return context.tourlogs.Where(t => t.time != null).ToList();
        }
        public int SearchForTourlogByID(int id)
        {
            var check = context.tourlogs.Where(t => t.id == id);
            return check.Count();
        }
        public ICollection<tourlogs> SearchForTourLogs(string searchtext)
        {
            return context.tourlogs.Where(t => t.time.Contains(searchtext) || t.distance.Contains(searchtext) || t.rating.Contains(searchtext) || t.comment.Contains(searchtext)).ToList();
        }
        public ICollection<tourlogs> GetAllTourLogsFromTour(int _tourid)
        {
            return context.tourlogs.Where(t => t.tourid == _tourid).ToList();
        }

        public void Remove(int id)
        {
            var t = context.tourlogs.Find(id);
            if (t != null)
            {
                context.tourlogs.Remove(t);
                context.SaveChanges();
            }
        }
        public tourlogs Update(tourlogs tourlog)
        {
            var entity = context.tourlogs.Find(tourlog.id);
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException("tour", "No tour found with that id to update!");
            }

            context.Entry(entity).CurrentValues.SetValues(tourlog);

            context.SaveChanges();

            return entity;
        }
    }
    public interface ITourLogRepository
    {
        tourlogs Add(tourlogs tourlog);
        tourlogs Update(tourlogs tourlog);
        void Remove(int id);
    }
}
