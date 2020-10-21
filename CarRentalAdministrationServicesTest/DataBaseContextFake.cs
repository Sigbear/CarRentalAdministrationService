using CarRentalAdministrationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarRentalAdministrationServicesTest
{
    class DataBaseContextFake : DataBaseContext
    {
        public DataBaseContextFake()
        {
            InitiateDb();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("testDb");
        }

        private void InitiateDb()
        {
            SeedCategories();
            SeedCars();
            SaveChanges();
        }

        private void SeedCategories()
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
            CarCategories.AddRange(carCategories);
        }

        private void SeedCars()
        {
            var cars = new[]
            {
                new Car
                {
                    CarId = 1,
                    Name = "DeLorean",
                    MileageInKilometers = 123,
                    CarCategory = CarCategories.Find(1),
                    Available = true
                },
                new Car
                {
                    CarId = 2,
                    Name = "Mystery Machine",
                    MileageInKilometers = 250,
                    CarCategory = CarCategories.Find(3),
                    Available = true,

                },
                new Car
                {
                    CarId = 3,
                    MileageInKilometers = 999,
                    Name = "BatMobile",
                    CarCategory = CarCategories.Find(2),
                    Available = false
                }
            };
            Cars.AddRange(cars);
        }
    }
}
