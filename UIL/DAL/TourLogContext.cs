using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TourLogContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<tourlogs> tourlogs { get; set; }

        public TourLogContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new[] { new LogInterceptor() });
            optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ToursContext"),
                                sopt => sopt.UseNetTopologySuite());
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");
        }
    }
}
