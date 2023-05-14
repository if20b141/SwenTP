using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class ToursContext : DbContext
    {
        

        private readonly IConfiguration _configuration;
        public DbSet<tours> tours { get; set; }

        public ToursContext(IConfiguration configuration)
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
