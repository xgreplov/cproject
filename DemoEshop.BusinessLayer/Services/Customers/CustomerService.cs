using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.QueryObjects.Common;
using DemoEshop.BusinessLayer.Services.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Services.Customers
{
    public class CustomerService : CrudQueryServiceBase<Customer, CustomerDto, CustomerFilterDto>, ICustomerService
    {
        public CustomerService(IMapper mapper, IRepository<Customer> customerRepository, QueryObjectBase<CustomerDto, Customer, CustomerFilterDto, IQuery<Customer>> customerQueryObject)
            : base(mapper, customerRepository, customerQueryObject) { }

        protected override async Task<Customer> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }
        
        /// <summary>
        /// Gets customer with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Customer with given email address</returns>
        public async Task<CustomerDto> GetCustomerAccordingToEmailAsync(string email)
        {
            var queryResult = await Query.ExecuteQuery(new CustomerFilterDto {Email = email});
            return queryResult.Items.SingleOrDefault();
        }

        public async Task<QueryResultDto<CustomerDto, CustomerFilterDto>> ListOnlyAllCustomersAsync()
        {
            var x = await Query.ExecuteQuery(new CustomerFilterDto{Roles = null});
            return x;
        }
    }
}
