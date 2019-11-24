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
    public class SongQueryObject : QueryObjectBase<SongDto, Song, SongFilterDto, IQuery<Song>>
    {
        public SongQueryObject(IMapper mapper, IQuery<Song> query) : base(mapper, query) { }

        protected override IQuery<Song> ApplyWhereClause(IQuery<Song> query, SongFilterDto filter)
        {
            var definedPredicates = new List<IPredicate>();
            AddIfDefined(FilterAlbum(filter), definedPredicates);
            AddIfDefined(FilterProductName(filter), definedPredicates);
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

        private static SimplePredicate FilterAlbum(SongFilterDto filter)
        {
            if (filter.AlbumId == null)
            {
                return null;
            }
            return new SimplePredicate(
                    nameof(Song.AlbumId),
                    ValueComparingOperator.Equal,
                    filter.AlbumId);
        }

        private static SimplePredicate FilterProductName(SongFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.SearchedName))
            {
                return null;
            }
            return new SimplePredicate(nameof(Song.Name), ValueComparingOperator.StringContains,
                filter.SearchedName);
        }

    }
}