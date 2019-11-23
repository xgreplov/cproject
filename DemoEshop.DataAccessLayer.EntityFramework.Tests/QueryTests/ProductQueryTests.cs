using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.DataAccessLayer.EntityFramework.Enums;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using DemoEshop.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.EntityFramework.Tests.QueryTests
{
    [TestFixture]
    public class ProductQueryTests
    {
        private readonly IUnitOfWorkProvider unitOfWorkProvider = Initializer.Container.Resolve<IUnitOfWorkProvider>();

        private readonly Guid androidCategoryId = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid samsungGalaxyJ7Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid lgG5Id = Guid.Parse("aa06dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid htc10Id = Guid.Parse("aa09dc64-5c07-40fe-a916-175165b9b90f");

        [Test]
        public async Task ExecuteAsync_SimpleWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>{new Song
            {
                Id = samsungGalaxyJ7Id,
                CategoryId = androidCategoryId,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                DiscountPercentage = 5,
                DiscountType = Genre.Percentage,
                Name = "Samsung Galaxy J7",
                Price = 7490,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg"
            }, new Song
            {
                Id = lgG5Id,
                CategoryId = androidCategoryId,
                Description = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            }, new Song
            {
                Id = htc10Id,
                CategoryId = androidCategoryId,
                Description = "HTC 10 smartphone was launched in April 2016. The phone comes with a 5.20-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 564 pixels per inch. The HTC 10 is powered by 1.6GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card. As far as the cameras are concerned, the HTC 10 packs a 12-Ultrapixel primary camera on the rear and a 5-megapixel front shooter for selfies.The HTC 10 runs Android 6 and is powered by a 3000mAh non removable battery.It measures 145.90 x 71.90 x 9.00 (height x width x thickness) and weighs 161.00 grams. The HTC 10 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "HTC 10",
                Price = 21990,
                ProductImgUri = @"\Content\Images\Products\HTC_10.jpg"
            }}, 3);

            var predicate = new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, androidCategoryId);
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_ComplexWherePredicate_ReturnsCorrectQueryResult()
        {
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>{new Song
            {
                Id = lgG5Id,
                CategoryId = androidCategoryId,
                Description = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            }}, 1);

            var predicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, androidCategoryId),
                new CompositePredicate(new List<IPredicate>
                {
                    new SimplePredicate(nameof(Song.DiscountPercentage), ValueComparingOperator.GreaterThanOrEqual, 0),
                    new SimplePredicate(nameof(Song.DiscountPercentage), ValueComparingOperator.LessThan, 5)
                }),
                new CompositePredicate(new List<IPredicate>
                {
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 12_000m),
                    new SimplePredicate(nameof(Song.Price), ValueComparingOperator.LessThanOrEqual, 16_000m)
                })
            });
            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.Where(predicate).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_RatingAllProductsByName_ReturnsCorrectlyOrderedQueryResult()
        {
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>{new Song
            {
                Id = htc10Id,
                CategoryId = androidCategoryId,
                Description = "HTC 10 smartphone was launched in April 2016. The phone comes with a 5.20-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 564 pixels per inch. The HTC 10 is powered by 1.6GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card. As far as the cameras are concerned, the HTC 10 packs a 12-Ultrapixel primary camera on the rear and a 5-megapixel front shooter for selfies.The HTC 10 runs Android 6 and is powered by a 3000mAh non removable battery.It measures 145.90 x 71.90 x 9.00 (height x width x thickness) and weighs 161.00 grams. The HTC 10 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "HTC 10",
                Price = 21990,
                ProductImgUri = @"\Content\Images\Products\HTC_10.jpg"
            }, new Song
            {
                Id = lgG5Id,
                CategoryId = androidCategoryId,
                Description = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            }, new Song
            {
                Id = samsungGalaxyJ7Id,
                CategoryId = androidCategoryId,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                DiscountPercentage = 5,
                DiscountType = Genre.Percentage,
                Name = "Samsung Galaxy J7",
                Price = 7490,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg"
            }}, 3);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.SortBy(nameof(Song.Price), false).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_RetrieveFirstProductsPage_ReturnsCorrectPage()
        {
            const int pageSize = 2;
            const int requestedPage = 1;
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>{new Song
            {
                Id = samsungGalaxyJ7Id,
                CategoryId = androidCategoryId,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                DiscountPercentage = 5,
                DiscountType = Genre.Percentage,
                Name = "Samsung Galaxy J7",
                Price = 7490,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg"
            }, new Song
            {
                Id = lgG5Id,
                CategoryId = androidCategoryId,
                Description = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                DiscountPercentage = 0,
                DiscountType = Genre.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            }}, 2, pageSize, requestedPage);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.Page(requestedPage, pageSize).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        [Test]
        public async Task ExecuteAsync_ComplexQuery_ReturnsCorrectResult()
        {
            const int pageSize = 5;
            const int requestedPage = 1;
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>
            {
                new Song
                {
                    Id = lgG5Id,
                    CategoryId = androidCategoryId,
                    Description =
                        "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                    DiscountPercentage = 0,
                    DiscountType = Genre.Percentage,
                    Name = "LG G5",
                    Price = 15490,
                    ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
                }
            }, 1, pageSize, requestedPage);
            var predicate = new CompositePredicate(
                new List<IPredicate>{
                    new SimplePredicate(nameof(Song.CategoryId), ValueComparingOperator.Equal, androidCategoryId),
                    new CompositePredicate(new List<IPredicate>
                    {
                        new SimplePredicate(nameof(Song.DiscountType), ValueComparingOperator.Equal,
                            Genre.Percentage),
                        new SimplePredicate(nameof(Song.DiscountPercentage), ValueComparingOperator.LessThan, 3),
                        new CompositePredicate(new List<IPredicate>
                        {
                            new SimplePredicate(nameof(Song.Price), ValueComparingOperator.GreaterThanOrEqual, 15_000m),
                            new SimplePredicate(nameof(Song.Price), ValueComparingOperator.LessThanOrEqual, 16_000m)
                        })
                    }),
                });

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.Where(predicate).SortBy(nameof(Song.Price), false).Page(requestedPage, pageSize).ExecuteAsync();
            }

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }


        [Test]
        public async Task ExecuteAsync_LikeOperator_ReturnsCorrectPage()
        {
            QueryResult<Song> actualQueryResult;
            var productQuery = Initializer.Container.Resolve<IQuery<Song>>();
            var expectedQueryResult = new QueryResult<Song>(new List<Song>{new Song
            {
                Id = samsungGalaxyJ7Id,
                CategoryId = androidCategoryId,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                DiscountPercentage = 5,
                DiscountType = Genre.Percentage,
                Name = "Samsung Galaxy J7",
                Price = 7490,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg"
            }}, 1);

            using (unitOfWorkProvider.Create())
            {
                actualQueryResult = await productQuery.Where(new SimplePredicate(nameof(Song.Name), ValueComparingOperator.StringContains, "Samsung")).ExecuteAsync();
            }
            
            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

    }
}
