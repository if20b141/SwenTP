using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DAL
{
	public class SampleContext : DbContext
	{
		public DbSet<User> Users { get; set; }
	}
}
