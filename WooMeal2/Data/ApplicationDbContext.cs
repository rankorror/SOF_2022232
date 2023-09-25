using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using WooMeal2.Models;

namespace WooMeal2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Meal> Meals { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DbSet<OrderMeal> OrderMeals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>()
                .HasOne(au => au.Restaurant)
                .WithOne(r => r.Owner)
                .HasForeignKey<Restaurant>(r => r.OwnerId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(au => au.Orders)
                .HasForeignKey(o => o.OwnerId);

            modelBuilder.Entity<Meal>()
                .HasOne(m => m.Owner)
                .WithMany(r => r.Meals)
                .HasForeignKey(m => m.OwnerId);

            modelBuilder.Entity<OrderMeal>()
                .HasKey(om => new { om.OrderId, om.MealId });

            modelBuilder.Entity<OrderMeal>()
                .HasOne(om => om.Order)
                .WithMany(o => o.Meals)
                .HasForeignKey(om => om.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderMeal>()
                .HasOne(om => om.Meal)
                .WithMany(m => m.Orders)
                .HasForeignKey(om => om.MealId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}