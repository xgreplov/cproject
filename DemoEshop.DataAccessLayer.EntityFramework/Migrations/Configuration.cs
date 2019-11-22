using System;
using System.Data.Entity.Migrations;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.DataAccessLayer.EntityFramework.Enums;

namespace DemoEshop.DataAccessLayer.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DemoEshopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DemoEshopDbContext context)
        {
            // Configure case invariant comparison for category and product names
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE Products ALTER COLUMN Name VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE Categories ALTER COLUMN Name VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL");

            // Password: PV226jesuper
            var admin = new User { Id = Guid.Parse("ab00dc64-5c07-40fe-a916-175165b9b90f"), Username = "MacakM", PasswordHash = "ZXnjeNKhDTSH6Rc6q4++tVoQVHo=", PasswordSalt = "hFrDVp5UB9eMycpU+4wSEA==", Roles = "Admin"};
            context.Users.AddOrUpdate(admin);

            // Password: qwerty123
            var customer = new Customer {Id = Guid.Parse("aa00dc64-5c07-40fe-a916-175165b9b90f"), Email = "daisy@gmail.com", FirstName = "Daisy", LastName = "Johnson", BirthDate = DateTime.Now.AddYears(-24), Address = "Wall Street, NY", MobilePhoneNumber = "123456789", Username = "Daisy1337", PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65/k=", PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==" };
            context.Customers.AddOrUpdate(customer);

            var smartphones = new Category { Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f"), Name = "Smartphones", Parent = null, ParentId = null };

            var android = new Category { Id = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f"), Name = "Android", Parent = smartphones, ParentId = smartphones.Id };

            var windows10Mobile = new Category { Id = Guid.Parse("aa03dc64-5c07-40fe-a916-175165b9b90f"), Name = "Windows 10", Parent = smartphones, ParentId = smartphones.Id };

            var iOS = new Category { Id = Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f"), Name = "iOS", Parent = smartphones, ParentId = smartphones.Id };

            context.Categories.AddOrUpdate(category => category.Id, smartphones, android, windows10Mobile, iOS);

            var samsungGalaxyJ7 = new Product
            {
                Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                StoredUnits = 6,
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
                StoredUnits = 0,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "LG G5",
                Price = 15490,
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg"
            };

            var lumia950XL = new Product
            {
                Id = Guid.Parse("aa07dc64-5c07-40fe-a916-175165b9b90f"),
                Category = windows10Mobile,
                CategoryId = windows10Mobile.Id,
                Description = "Microsoft Lumia 950 XL smartphone was launched in October 2015. The phone comes with a 5.70-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 518 pixels per inch. The Microsoft Lumia 950 XL is powered by octa - core Qualcomm Snapdragon 810 processor and it comes with 3GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the Microsoft Lumia 950 XL packs a 20-megapixel primary camera on the rear and a 5-megapixel front shooter for selfies. The Microsoft Lumia 950 XL runs Windows 10 Mobile and is powered by a 3340mAh removable battery.It measures 151.90 x 78.40 x 8.10 (height x width x thickness) and weighs 165.00 grams. The Microsoft Lumia 950 XL is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                StoredUnits = 8,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "Microsoft Lumia 950XL",
                Price = 16490,
                ProductImgUri = @"\Content\Images\Products\microsoft_lumia_950_xl.jpeg"
            };
            var lumia650 = new Product
            {
                Id = Guid.Parse("aa08dc64-5c07-40fe-a916-175165b9b90f"),
                Category = windows10Mobile,
                CategoryId = windows10Mobile.Id,
                Description = "Microsoft Lumia 650 smartphone was launched in February 2016. The phone comes with a 5.00-inch touchscreen display with a resolution of 720 pixels by 1280 pixels at a PPI of 297 pixels per inch. The Microsoft Lumia 650 is powered by 1.3GHz quad - core Qualcomm Snapdragon 212 processor and it comes with 1GB of RAM.The phone packs 16GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the Microsoft Lumia 650 packs a 8-megapixel primary camera on the rear and a 5-megapixel front shooter for selfies. The Microsoft Lumia 650 runs Windows 10 Mobile and is powered by a 2000mAh removable battery.It measures 70.90 x 142.00 x 6.90 (height x width x thickness) and weighs 122.00 grams. The Microsoft Lumia 650 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer. ",
                StoredUnits = 7,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Value3Plus1,
                Name = "Microsoft Lumia 650",
                Price = 5990,
                ProductImgUri = @"\Content\Images\Products\microsoft_lumia_650.jpeg"
            };
            var htc10 = new Product
            {
                Id = Guid.Parse("aa09dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "HTC 10 smartphone was launched in April 2016. The phone comes with a 5.20-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 564 pixels per inch. The HTC 10 is powered by 1.6GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card. As far as the cameras are concerned, the HTC 10 packs a 12-Ultrapixel primary camera on the rear and a 5-megapixel front shooter for selfies.The HTC 10 runs Android 6 and is powered by a 3000mAh non removable battery.It measures 145.90 x 71.90 x 9.00 (height x width x thickness) and weighs 161.00 grams. The HTC 10 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                StoredUnits = 1,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "HTC 10",
                Price = 21990,
                ProductImgUri = @"\Content\Images\Products\HTC_10.jpg"
            };
            var zenfone5 = new Product
            {
                Id = Guid.Parse("aa10dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "Asus ZenFone 5 smartphone was launched in January 2014. The phone comes with a 5.00-inch touchscreen display with a resolution of 720 pixels by 1280 pixels The Asus ZenFone 5 is powered by 1.6GHz dual - core Intel Atom Z2560 processor and it comes with 2GB of RAM.The phone packs 8GB of internal storage that can be expanded up to 64GB via a microSD card.As far as the cameras are concerned, the Asus ZenFone 5 packs a 8-megapixel primary camera on the rear and a 2-megapixel front shooter for selfies. The Asus ZenFone 5 runs Android 4.3 and is powered by a 2110mAh non removable battery.It measures 148.20 x 72.80 x 10.34 (height x width x thickness) and weighs 145.00 grams. The Asus ZenFone 5 is a dual SIM(GSM and GSM) smartphone that accepts two Micro-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, FM.Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer. ",
                StoredUnits = 0,
                DiscountPercentage = 21,
                DiscountType = DiscountType.Percentage,
                Name = "Asus ZenFone 5",
                Price = 5490,
                ProductImgUri = @"\Content\Images\Products\asus_zenfone_5.jpeg"
            };
            var xperiaZ5 = new Product
            {
                Id = Guid.Parse("aa11dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "Sony Xperia Z5 Premium comes with a 5.50-inch touchscreen display with a resolution of 2160 pixels by 3840 pixels at a PPI of 806 pixels per inch. The Sony Xperia Z5 Premium is powered by octa - core Qualcomm Snapdragon 810(MSM8994) processor and it comes with 3GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the Sony Xperia Z5 Premium Dual packs a 23-megapixel primary camera on the rear and a 5-megapixel front shooter for selfies. The Sony Xperia Z5 Premium Dual runs Android 5.1 and is powered by a 3430mAh non removable battery.It measures 154.40 x 76.00 x 7.80 (height x width x thickness) and weighs 180.00 grams. The Sony Xperia Z5 Premium Dual is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                StoredUnits = 2,
                DiscountPercentage = 5,
                DiscountType = DiscountType.Percentage,
                Name = "Sony Xperia Z5 Premium",
                Price = 20490,
                ProductImgUri = @"\Content\Images\Products\sony_xperia_z5_premium.jpeg"
            };
            var xiaomiMi5 = new Product
            {
                Id = Guid.Parse("aa12dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "Xiaomi Mi 5 smartphone was launched in February 2016. The phone comes with a 5.15-inch touchscreen display with a resolution of 1080 pixels by 1920 pixels at a PPI of 428 pixels per inch. The Xiaomi Mi 5 is powered by 1.3GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 3GB of RAM.The phone packs 32GB of internal storage cannot be expanded.As far as the cameras are concerned, the Xiaomi Mi 5 packs a 16-megapixel primary camera on the rear and a 4-megapixel front shooter for selfies. The Xiaomi Mi 5 runs Android 6.0 and is powered by a 3000mAh non removable battery. It measures 144.50 x 69.20 x 7.25 (height x width x thickness) and weighs 129.00 grams. The Xiaomi Mi 5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                StoredUnits = 1,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "Xiaomi Mi 5",
                Price = 11990,
                ProductImgUri = @"\Content\Images\Products\xiaomi_mi_5.jpeg"
            };
            var blackberryPriv = new Product
            {
                Id = Guid.Parse("aa13dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "BlackBerry Priv smartphone comes with a 5.40-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 540 pixels per inch. The BlackBerry Priv is powered by 1.44GHz hexa - core Qualcomm Snapdragon 808 processor and it comes with 3GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card.As far as the cameras are concerned, the BlackBerry Priv packs a 18-megapixel primary camera on the rear and a 2-megapixel front shooter for selfies. The BlackBerry Priv runs Android 5.1.1 and is powered by a 3410mAh non removable battery.It measures 147.00 x 77.20 x 9.40 (height x width x thickness) and weighs 192.00 grams. The BlackBerry Priv is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                StoredUnits = 0,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "BlackBerry Priv",
                Price = 18990,
                ProductImgUri = @"\Content\Images\Products\blackberry_priv.jpg"
            };
            var nubiaZ9Exclusive = new Product
            {
                Id = Guid.Parse("aa14dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Description = "The phone comes with a 5.20-inch touchscreen display with a resolution of 1080 pixels by 1920 pixels at a PPI of 424 pixels per inch. The ZTE Nubia Z9 Exclusive is powered by 1.5GHz octa - core Qualcomm Snapdragon 810 processor and it comes with 4GB of RAM.The phone packs 64GB of internal storage cannot be expanded.As far as the cameras are concerned, the ZTE Nubia Z9 Exclusive packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The ZTE Nubia Z9 Exclusive runs Android 5.0 and is powered by a 2900mAh non removable battery. It measures 147.40 x 68.30 x 8.90 (height x width x thickness) and weighs 192.00 grams. The ZTE Nubia Z9 Exclusive is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Ambient light sensor, Accelerometer, and Gyroscope. ",
                StoredUnits = 1,
                DiscountPercentage = 7,
                DiscountType = DiscountType.Percentage,
                Name = "ZTE Nubia Z9 Exclusive",
                Price = 23490,
                ProductImgUri = @"\Content\Images\Products\zte-nubia-z9-exclusive.jpg"
            };
            var iPhoneSE = new Product
            {
                Id = Guid.Parse("aa15dc64-5c07-40fe-a916-175165b9b90f"),
                Category = iOS,
                CategoryId = iOS.Id,
                Description = "The phone comes with a 4.00-inch touchscreen display with a resolution of 640 pixels by 1136 pixels at a PPI of 326 pixels per inch. The Apple iPhone SE is powered by A9 processor and it comes with 2GB of RAM.The phone packs 64GB of internal storage cannot be expanded. As far as the cameras are concerned, the Apple iPhone SE packs a 12-megapixel primary camera on the rear and a 1.2-megapixel front shooter for selfies. The Apple iPhone SE runs iOS 9.3. It measures 123.80 x 58.60 x 7.66 (height x width x thickness) and weighs 113.00 grams. The Apple iPhone SE is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                StoredUnits = 5,
                DiscountPercentage = 10,
                DiscountType = DiscountType.Percentage,
                Name = "Apple iPhone SE",
                Price = 16990,
                ProductImgUri = @"\Content\Images\Products\3222016120930AM_635_apple_iphone_se.jpeg"
            };
            var iPhone6sPlus = new Product
            {
                Id = Guid.Parse("aa16dc64-5c07-40fe-a916-175165b9b90f"),
                Category = iOS,
                CategoryId = iOS.Id,
                Description = "Apple iPhone 6s Plus smartphone comes with a 5.50-inch touchscreen display with a resolution of 1080 pixels by 1920 pixels at a PPI of 401 pixels per inch. The Apple iPhone 6s Plus is powered by A9 processor and it comes with 2GB of RAM.The phone packs 64GB of internal storage cannot be expanded.As far as the cameras are concerned, the Apple iPhone 6s Plus packs a 12-megapixel primary camera on the rear and a 5-megapixel front shooter for selfies. The Apple iPhone 6s Plus runs iOS 9 and is powered by a 2750mAh non removable battery. It measures 158.20 x 77.90 x 7.30 (height x width x thickness) and weighs 192.00 grams. The Apple iPhone 6s Plus is a single SIM(GSM) smartphone that accepts a Nano-SIM. Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                StoredUnits = 4,
                DiscountPercentage = 0,
                DiscountType = DiscountType.Percentage,
                Name = "Apple iPhone 6s Plus",
                Price = 29990,
                ProductImgUri = @"\Content\Images\Products\910201513146AM_635_apple_iphone_6s_plus.jpeg"
            };

            context.Products.AddOrUpdate(product => product.Id, samsungGalaxyJ7, lgG5, htc10, zenfone5, xperiaZ5, xiaomiMi5, blackberryPriv, nubiaZ9Exclusive, lumia950XL, lumia650, iPhoneSE, iPhone6sPlus);

            context.SaveChanges();
        }
    }
}
