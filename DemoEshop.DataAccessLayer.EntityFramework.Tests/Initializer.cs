using System.Data.Entity;
using Castle.Windsor;
using DemoEshop.DataAccessLayer.EntityFramework.Tests.Config;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.EntityFramework.Tests
{
    [SetUpFixture]
    public class Initializer
    {
        internal static readonly IWindsorContainer Container = new WindsorContainer();

        /// <summary>
        /// Initializes all Business Layer tests
        /// </summary>
        [OneTimeSetUp]
        public void InitializeBusinessLayerTests()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DemoEshopDbContext>());          
            Container.Install(new EntityFrameworkTestInstaller());
        }
    }
}
