using System;
using AsyncPoco;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.PetaPoco;
using DemoEshop.Infrastructure.PetaPoco.UnitOfWork;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.DataAccessLayer.PetaPoco.Config
{
    public class PetaPocoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<IDatabase>>()
                    .Instance(() => new Database("Data source=(localdb)\\mssqllocaldb;Database=DemoEshopDatabaseSample;Trusted_Connection=True;MultipleActiveResultSets=true", "System.Data.SqlClient"))
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
    }
}
