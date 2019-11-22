using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades.Common
{
    public abstract class FacadeBase
    {
        protected readonly IUnitOfWorkProvider UnitOfWorkProvider;

        protected FacadeBase(IUnitOfWorkProvider unitOfWorkProvider)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
