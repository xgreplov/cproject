using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Customers;
using DemoEshop.Infrastructure.UnitOfWork;
using DemoEshop.BusinessLayer.Services.Users;
using System;

namespace DemoEshop.BusinessLayer.Facades
{
    public class CustomerFacade : FacadeBase
    {
        private readonly ICustomerService customerService;
        private readonly IUserService userService;

        public CustomerFacade(IUnitOfWorkProvider unitOfWorkProvider, ICustomerService customerService, IUserService userService) : base(unitOfWorkProvider)
        {
            this.customerService = customerService;
            this.userService = userService;
        }
        
        /// <summary>
        /// Gets customer according to email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Customer with specified email</returns>
        public async Task<CustomerDto> GetCustomerAccordingToEmailAsync(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await customerService.GetCustomerAccordingToEmailAsync(email);
            }          
        }

        public async Task<CustomerDto> GetCustomerAccordingToUsernameAsync(string username)
        {
            UserDto user;
            using (var unitOfWork = UnitOfWorkProvider.Create())
            {
                user = await userService.GetUserAccordingToUsernameAsync(username);
                //await unitOfWork.Commit();

                return await customerService.GetAsync(user.Id);
            }
            /*using (UnitOfWorkProvider.Create())
            {
                return await customerService.GetAsync(user.Id);
            }*/

        }

        /// <summary>
        /// Gets all customers according to page
        /// </summary>
        /// <returns>all customers</returns>
        public async Task<QueryResultDto<CustomerDto, CustomerFilterDto>> GetAllCustomersAsync()
        {
            using (UnitOfWorkProvider.Create())
            {
                return await customerService.ListOnlyAllCustomersAsync();
            }
        }

        /// <summary>
        /// Performs customer registration
        /// </summary>
        /// <param name="userCreateDto">Customer registration details</param>
        /// <returns>Registered customer account ID</returns>
        public async Task<Guid> RegisterCustomer(UserCreateDto userCreateDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                try
                {
                    var id = await userService.RegisterUserAsync(userCreateDto);
                    await uow.Commit();
                    return id;
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }

        public async Task<(bool success, string roles)> Login(string username, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.AuthorizeUserAsync(username, password);
            }
        }

        public async Task<UserDto> GetUserAccordingToUsernameAsync(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.GetUserAccordingToUsernameAsync(username);
            }
        }
    }
}
