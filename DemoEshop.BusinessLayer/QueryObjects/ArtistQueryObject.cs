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
    public class ArtistQueryObject : QueryObjectBase<ArtistDto, Artist, ArtistFilterDto, IQuery<Artist>>
    {
        public ArtistQueryObject(IMapper mapper, IQuery<Artist> query) : base(mapper, query) { }

        protected override IQuery<Artist> ApplyWhereClause(IQuery<Artist> query, ArtistFilterDto filter)
        {
            return query.Where(new SimplePredicate(nameof(Artist.Name), ValueComparingOperator.Equal, filter.Name));
        }
    }
}
