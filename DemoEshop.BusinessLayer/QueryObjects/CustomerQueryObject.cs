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
    public class CustomerQueryObject : QueryObjectBase<CustomerDto, Customer, CustomerFilterDto, IQuery<Customer>>
    {
        public CustomerQueryObject(IMapper mapper, IQuery<Customer> query) : base(mapper, query) { }

        protected override IQuery<Customer> ApplyWhereClause(IQuery<Customer> query, CustomerFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(new SimplePredicate(nameof(Customer.Email), ValueComparingOperator.Equal, filter.Email));
            }
            return query.Where(new SimplePredicate(nameof(Customer.Roles), ValueComparingOperator.Equal, filter.Roles));
        }
    }
}
