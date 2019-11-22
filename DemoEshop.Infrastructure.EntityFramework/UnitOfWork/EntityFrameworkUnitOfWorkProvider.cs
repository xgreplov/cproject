using System;
using System.Data.Entity;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.Infrastructure.EntityFramework.UnitOfWork
{
    public class EntityFrameworkUnitOfWorkProvider : UnitOfWorkProviderBase
    {
        private readonly Func<DbContext> dbContextFactory;

        public EntityFrameworkUnitOfWorkProvider(Func<DbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public override IUnitOfWork Create()
        {
            UowLocalInstance.Value = new EntityFrameworkUnitOfWork(dbContextFactory);
            return UowLocalInstance.Value;
        }
    }
}
