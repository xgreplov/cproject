using System;
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
    public class OrderQueryObject : QueryObjectBase<RatingDto, Rating, RatingFilterDto, IQuery<Rating>>
    {
        public OrderQueryObject(IMapper mapper, IQuery<Rating> query) : base(mapper, query) { }

        protected override IQuery<Rating> ApplyWhereClause(IQuery<Rating> query, RatingFilterDto filter)
        {
            return filter.CustomerId.Equals(Guid.Empty)
                ? query
                : query.Where(new SimplePredicate(nameof(Rating.CustomerId), ValueComparingOperator.Equal, filter.CustomerId));
        }
    }
}
