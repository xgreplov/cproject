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
    public class AlbumQueryObject : QueryObjectBase<CategoryDto, Category, CategoryFilterDto, IQuery<Category>>
    {
        public AlbumQueryObject(IMapper mapper, IQuery<Category> query) : base(mapper, query) { }

        protected override IQuery<Category> ApplyWhereClause(IQuery<Category> query, AlbumFilterDto filter)
        {
            if (filter.Names == null || !filter.Names.Any())
            {
                return query;
            }
            var albumNamePredicates = new List<IPredicate>(filter.Names
                .Select(name => new SimplePredicate(
                    nameof(Album.Name),
                    ValueComparingOperator.Equal,
                    name)));
            var predicate = new CompositePredicate(albumNamePredicates, LogicalOperator.OR);
            return query.Where(predicate);
        }
    }
}
