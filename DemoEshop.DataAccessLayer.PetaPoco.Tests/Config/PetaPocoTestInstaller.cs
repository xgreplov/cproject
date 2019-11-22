using System;
using System.Data.SQLite;
using AsyncPoco;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DemoEshop.DataAccessLayer.PetaPoco.Entities;
using DemoEshop.DataAccessLayer.PetaPoco.Enums;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.PetaPoco;
using DemoEshop.Infrastructure.PetaPoco.UnitOfWork;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.DataAccessLayer.PetaPoco.Tests.Config
{
    internal class PetaPocoTestInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<IDatabase>>()
                    .Instance(InitializeDatabase)
                    .LifestyleTransient(),
                Component.For<IUnitOfWorkProvider>()
                    .ImplementedBy<PetaPocoUnitOfWorkProvider>()
                    .LifestyleSingleton(),
                Component.For(typeof(IRepository<>))
                    .ImplementedBy(typeof(PetaPocoRepository<>))
                    .LifestyleTransient(),
                Component.For(typeof(IQuery<>))
                    .ImplementedBy(typeof(PetaPocoQuery<>))
                    .LifestyleTransient()
            );
        }

        /// <summary>
        /// Creates in-memory DB seeded with basic data
        /// </summary>
        /// <returns>the in-memory database instance</returns>
        private IDatabase InitializeDatabase()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();
            var database = new Database(connection);
            database.ExecuteAsync("CREATE TABLE  Categories(Id GUID PRIMARY KEY, Name TEXT, ParentId GUID, FOREIGN KEY(ParentId) REFERENCES Categories(Id));").Wait();
            database.ExecuteAsync("CREATE TABLE  Products(Id GUID PRIMARY KEY, Name TEXT, StoredUnits INT, Description TEXT, Price DECIMAL(10, 5), DiscountType INT, " +
                                  "DiscountPercentage INT, ProductImgUri TEXT, CategoryId GUID, FOREIGN KEY(CategoryId) REFERENCES Categories(Id));").Wait();

            var smartphones = new Category { Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f"), Name = "Smartphones" };
            var android = new Category { Id = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f"), Name = "Android", Parent = smartphones, ParentId = smartphones.Id };
            var iOS = new Category { Id = Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f"), Name = "iOS", Parent = smartphones, ParentId = smartphones.Id };
            database.InsertAsync(smartphones).Wait();
            database.InsertAsync(android).Wait();
            database.InsertAsync(iOS).Wait();
            var samsungGalaxyJ7 = new Product
            {
                Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                DiscountPercentage = 5,
                DiscountType = DiscountType.Percentage,
                Name = "Samsung Galaxy J7",
                Price = 7490,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg"
            };

            var lgG5 = new Product
            {
                Id = Guid.Parse("aa06dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            };

            var htc10 = new Product
            {
                Id = Guid.Parse("aa09dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "HTC 10 smartphone was launched in April 2016. The phone comes with a 5.20-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 564 pixels per inch. The HTC 10 is powered by 1.6GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card. As far as the cameras are concerned, the HTC 10 packs a 12-Ultrapixel primary camera on the rear and a 5-megapixel front shooter for selfies.The HTC 10 runs Android 6 and is powered by a 3000mAh non removable battery.It measures 145.90 x 71.90 x 9.00 (height x width x thickness) and weighs 161.00 grams. The HTC 10 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "HTC 10",
                Price = 21990,
                ProductImgUri = @"\Content\Images\Products\HTC_10.jpg"
            };

            database.InsertAsync(samsungGalaxyJ7).Wait();
            database.InsertAsync(lgG5).Wait();
            database.InsertAsync(htc10).Wait();

            return database;
        }
    }
}
