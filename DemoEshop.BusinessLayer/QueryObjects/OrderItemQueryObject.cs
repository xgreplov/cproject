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
    public class RateSongQueryObject : QueryObjectBase<RateSongDto, RateSong, RateSongFilterDto, IQuery<RateSong>>
    {
        public RateSongQueryObject(IMapper mapper, IQuery<RateSong> query) : base(mapper, query) { }

        protected override IQuery<RateSong> ApplyWhereClause(IQuery<RateSong> query, RateSongFilterDto filter)
        {
            if (filter.RateSongId.Equals(Guid.Empty))
            {
                return query;
            }
            return query.Where(new SimplePredicate(nameof(RateSong.RatingId), ValueComparingOperator.Equal, filter.RateSongId));
        }
    }
}
