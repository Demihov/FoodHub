using FoodHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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

			builder.Entity<Product>().HasKey(p => p.Id);

			builder.Entity<CartItem>()
				.HasOne(ci => ci.Product)
				.WithMany()
				.HasForeignKey(ci => ci.ProductId);

			builder.Entity<Cart>()
			    .HasMany(c => c.CartItems)
			    .WithOne()
			    .HasForeignKey(ci => ci.CartId);
		}

		public DbSet<BusinessUser> BusinessUsers { get; set; }
		public DbSet<CustomerUser> CustomerUsers { get; set; }

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
	}
}
