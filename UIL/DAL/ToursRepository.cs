using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class ToursNOSQLRepository : IToursRepository
    {
        public ICollection<tours> GetAllTours()
        {
            throw new System.NotImplementedException();
        }
        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }
        public tours Add(tours tour)
        {
            throw new System.NotImplementedException();
        }
        public tours Update(tours tour)
        {
            throw new System.NotImplementedException();
        }

    }
    public class ToursSQLRepository : IToursRepository
    {
        private readonly ToursContext context;
        public ToursSQLRepository(ToursContext context)
        {
            this.context = context;
        }
        public tours Add(tours tour)
        {
            context.Add(tour);
            context.SaveChanges();

            return tour;
        }
        public ICollection<tours> GetAllTours()
        {
            return context.tours.Where(t => t.tourname != null).ToList();
        }

        public void Remove(int id)
        {
            var t = context.tours.Find(id);
            if(t != null)
            {
                context.tours.Remove(t);
                context.SaveChanges();
            }
        }
        public tours Update(tours tour)
        {
            var entity = context.tours.Find(tour.id);
            if(entity == null)
            {
                throw new ArgumentOutOfRangeException("tour", "No tour found with that id to update!");
            }

            context.Entry(entity).CurrentValues.SetValues(tour);

            context.SaveChanges();

            return entity;
        }
    }
    public interface IToursRepository
    {
        tours Add(tours tour);
        tours Update(tours tour);
        void Remove(int id);
    }
}
