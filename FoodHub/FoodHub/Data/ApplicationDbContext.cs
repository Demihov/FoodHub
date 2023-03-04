using FoodHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodHub.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		    : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<BusinessUser>()
				.ToTable("BusinessUsers");
		}

		public DbSet<BusinessUser> BusinessUsers { get; set; }
		public DbSet<CustomerUser> CustomerUsers { get; set; }
	}
}
