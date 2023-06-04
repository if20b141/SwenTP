using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PdfSharp.Pdf.Content.Objects;

namespace DAL
{


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
        public ICollection<tours> SearchForTours(string searchtext)
        {
            return context.tours.Where(t => t.tourname.Contains(searchtext) || t.description.Contains(searchtext) || t.startpoint.Contains(searchtext) || t.endpoint.Contains(searchtext) || t.type.Contains(searchtext) || t.distance.Contains(searchtext) || t.time.Contains(searchtext)).ToList();
        }
        public ICollection<tours> SelectTour(string tourname)
        {
            return context.tours.Where(t => t.tourname == tourname).ToList<tours>();
        }
        public ICollection<tours> GetSpecificTourByID(int id)
        {
            return context.tours.Where(t => t.id == id).ToList<tours>();
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
        ICollection<tours> GetSpecificTourByID(int id);
        ICollection<tours> GetAllTours();
        ICollection<tours> SelectTour(string tourname);
    }
}
