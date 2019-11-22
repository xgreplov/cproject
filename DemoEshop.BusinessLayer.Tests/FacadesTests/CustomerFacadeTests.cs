using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades;
using DemoEshop.BusinessLayer.Services.Customers;
using DemoEshop.BusinessLayer.Services.Users;
using DemoEshop.BusinessLayer.Tests.FacadesTests.Common;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using NUnit.Framework;

namespace DemoEshop.BusinessLayer.Tests.FacadesTests
{
    [TestFixture]
    public class CustomerFacadeTests
    {
        [Test]
        public async Task GetCustomerAccordingToEmailAsync_ExistingCustomer_ReturnsCorrectCustomer()
        {
            const string customerEmail = "user@somewhere.com";
            var expectedCustomer = new CustomerDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = customerEmail
            };
            var expectedQueryResult = new QueryResultDto<CustomerDto, CustomerFilterDto> {Items = new List<CustomerDto> {expectedCustomer}};
            var customerFacade = CreateCustomerFacade(expectedQueryResult);

            var actualCustomer = await customerFacade.GetCustomerAccordingToEmailAsync(customerEmail);

            Assert.AreEqual(actualCustomer, expectedCustomer);
        }
        
        [Test]
        public async Task GetAllCustomersAsync_TwoExistingCustomers_ReturnsCorrectQueryResult()
        {
            var expectedQueryResult = new QueryResultDto<CustomerDto, CustomerFilterDto>
            {
                Filter = new CustomerFilterDto(),
                Items = new List<CustomerDto> {new CustomerDto{Id = Guid.NewGuid()}, new CustomerDto { Id = Guid.NewGuid() }},
                PageSize = 10,
                RequestedPageNumber = null

            };
            var customerFacade = CreateCustomerFacade(expectedQueryResult);

            var actualQueryResult = await customerFacade.GetAllCustomersAsync();

            Assert.AreEqual(actualQueryResult, expectedQueryResult);
        }

        private static CustomerFacade CreateCustomerFacade(QueryResultDto<CustomerDto, CustomerFilterDto> expectedQueryResult)
        {
            var mockManager = new FacadeMockManager();
            var uowMock = FacadeMockManager.ConfigureUowMock();
            var mapper = FacadeMockManager.ConfigureRealMapper();
            var customerRepositoryMock = mockManager.ConfigureRepositoryMock<Customer>();
            var userRepositoryMock = mockManager.ConfigureRepositoryMock<User>();
            var customerQueryMock = mockManager.ConfigureQueryObjectMock<CustomerDto, Customer, CustomerFilterDto>(expectedQueryResult);
            var userQueryMock = mockManager.ConfigureQueryObjectMock<UserDto, User, UserFilterDto>(null);
            var customerService = new CustomerService(mapper, customerRepositoryMock.Object, customerQueryMock.Object);
            var userService = new UserService(mapper, customerRepositoryMock.Object, userQueryMock.Object);
            var customerFacade = new CustomerFacade(uowMock.Object, customerService, userService);
            return customerFacade;
        }
    }
}
