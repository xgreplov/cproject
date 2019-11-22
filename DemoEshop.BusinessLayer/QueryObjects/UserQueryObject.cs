using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.Infrastructure.Query;
using AutoMapper;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;

namespace DemoEshop.BusinessLayer.QueryObjects
{
    public class UserQueryObject : QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>>
    {
        public UserQueryObject(IMapper mapper, IQuery<User> query) : base(mapper, query)
        {
        }

        protected override IQuery<User> ApplyWhereClause(Infrastructure.Query.IQuery<User> query, UserFilterDto filter)
        {
            return query.Where(new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filter.Username));
        }
    }
}
