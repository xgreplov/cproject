using Castle.Windsor;
using DemoEshop.DataAccessLayer.PetaPoco.Tests.Config;
using NUnit.Framework;

namespace DemoEshop.DataAccessLayer.PetaPoco.Tests
{
    /// <summary>
    /// Main test initializer class
    /// </summary>
    [SetUpFixture]
    public class Initializer
    {
        internal static readonly IWindsorContainer Container = new WindsorContainer();

        /// <summary>
        /// Initializes all PetaPoco tests
        /// </summary>
        [OneTimeSetUp]
        public void InitializePetaPocoTests()
        {
            Container.Install(new PetaPocoTestInstaller());
        }
    }
}
