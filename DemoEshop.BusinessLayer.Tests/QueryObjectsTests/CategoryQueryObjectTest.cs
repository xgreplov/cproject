using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects;
using DemoEshop.BusinessLayer.Tests.QueryObjectsTests.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using NUnit.Framework;

namespace DemoEshop.BusinessLayer.Tests.QueryObjectsTests
{
    [TestFixture]
    public class CategoryQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_SimpleFilterWithMinimalPrice_ReturnsCorrectSimplePredicate()
        {
            const string desiredName1 = "Android";
            const string desiredName2 = "iOS";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Category.Name), ValueComparingOperator.Equal, desiredName1),
                    new SimplePredicate(nameof(Category.Name), ValueComparingOperator.Equal, desiredName2)
                }, LogicalOperator.OR);
            var mapperMock = mockManager.ConfigureMapperMock<Category, CategoryDto, CategoryFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Category>();
            var categoryQueryObject = new CategoryQueryObject(mapperMock.Object, queryMock.Object);

            var unused = await categoryQueryObject.ExecuteQuery(new CategoryFilterDto { Names = new []{desiredName1, desiredName2}});

            Assert.AreEqual(mockManager.CapturedPredicate, expectedPredicate);
        }

    }
}
