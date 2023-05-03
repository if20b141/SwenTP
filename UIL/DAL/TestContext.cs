using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class TestContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

    }
}
