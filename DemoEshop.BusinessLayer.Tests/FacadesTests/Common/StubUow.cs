using System.Threading.Tasks;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Tests.FacadesTests.Common
{
    internal class StubUow : UnitOfWorkBase
    {
        protected override Task CommitCore()
        {
            return Task.Delay(15);
        }

        public override void Dispose()
        {
        }
    }
}
