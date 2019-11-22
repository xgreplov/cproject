using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;

namespace DemoEshop.BusinessLayer.QueryObjects
{
    public class ProductQueryObject : QueryObjectBase<ProductDto, Product, ProductFilterDto, IQuery<Product>>
    {
        public ProductQueryObject(IMapper mapper, IQuery<Product> query) : base(mapper, query) { }

        protected override IQuery<Product> ApplyWhereClause(IQuery<Product> query, ProductFilterDto filter)
        {
            var definedPredicates = new List<IPredicate>();
            AddIfDefined(FilterCategories(filter), definedPredicates);
            AddIfDefined(FilterProductName(filter), definedPredicates);
            AddIfDefined(FilterProductPrice(filter), definedPredicates);
            if (definedPredicates.Count == 0)
            {
                return query;
            }
            if (definedPredicates.Count == 1)
            {
                return query.Where(definedPredicates.First());
            }
            var wherePredicate = new CompositePredicate(definedPredicates);
            return query.Where(wherePredicate);
        }

        private static void AddIfDefined(IPredicate categoryPredicate, ICollection<IPredicate> definedPredicates)
        {
            if (categoryPredicate != null)
            {
                definedPredicates.Add(categoryPredicate);
            }
        }

        private static CompositePredicate FilterCategories(ProductFilterDto filter)
        {
            if (filter.CategoryIds == null || !filter.CategoryIds.Any())
            {
                return null;
            }
            var categoryIdPredicates = new List<IPredicate>(filter.CategoryIds
                .Select(categoryId => new SimplePredicate(
                    nameof(Product.CategoryId),
                    ValueComparingOperator.Equal,
                    categoryId)));
            return new CompositePredicate(categoryIdPredicates, LogicalOperator.OR);
        }

        private static SimplePredicate FilterProductName(ProductFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.SearchedName))
            {
                return null;
            }
            return new SimplePredicate(nameof(Product.Name), ValueComparingOperator.StringContains,
                filter.SearchedName);
        }

        private static IPredicate FilterProductPrice(ProductFilterDto filter)
        {
            if (filter.MinimalPrice == 0 && filter.MaximalPrice == decimal.MaxValue)
            {
                return null;
            }
            if (filter.MinimalPrice > 0 && filter.MaximalPrice < decimal.MaxValue)
            {
                return new CompositePredicate(new List<IPredicate>
                {
                    new SimplePredicate(nameof(Product.Price), ValueComparingOperator.GreaterThanOrEqual,
                        filter.MinimalPrice),
                    new SimplePredicate(nameof(Product.Price), ValueComparingOperator.LessThanOrEqual,
                        filter.MaximalPrice),
                });
            }
            if (filter.MinimalPrice > 0)
            {
                return new SimplePredicate(nameof(Product.Price), ValueComparingOperator.GreaterThanOrEqual, filter.MinimalPrice);
            }
            // filter.MaximalPrice < decimal.MaxValue
            return new SimplePredicate(nameof(Product.Price), ValueComparingOperator.LessThanOrEqual, filter.MaximalPrice);
        }
    }
}
