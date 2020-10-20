using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarRentalAdministrationService.Model
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var carCategories = new[]
            {
                new CarCategory
                {
                    CarCategoryId = 1,
                    Category = "Compact",
                    BaseDayRentalCost = 100,
                    KilometerPrice = 10
                },
                new CarCategory
                {
                    CarCategoryId = 2,
                    Category = "Premium",
                    BaseDayRentalCost = 125,
                    KilometerPrice = 12
                },
                new CarCategory
                {
                    CarCategoryId = 3,
                    Category = "Minivan",
                    BaseDayRentalCost = 150,
                    KilometerPrice = 15
                }
            };

            modelBuilder.Entity<CarCategory>().HasData(carCategories);

            //modelBuilder.Entity<Car>().HasData(
            //    new
            //    {
            //        CarId = 1,
            //        Name = "DeLorean",
            //        MileageInKilometers = 123,
            //        CarCategory = 1,
            //        Available = true
            //    },
            //    new
            //    {
            //        CarId = 2,
            //        Name = "Mystery Machine",
            //        MileageInKilometers = 250,
            //        CarCategory = 3,
            //        Available = true,

            //    },
            //    new
            //    {
            //        CarId = 3,
            //        MileageInKilometers = 999,
            //        Name = "BatMobile",
            //        CarCategory = 2,
            //        Available = false
            //    }
            //);
        }
    }
}
