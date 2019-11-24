using System;
using System.Threading.Tasks;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;
using DemoEshop.BusinessLayer.Facades.Common;
using DemoEshop.BusinessLayer.Services.Users;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.BusinessLayer.Facades
{
    public class UserFacade : FacadeBase
    {
        private readonly IUserService userService;
        
        public UserFacade(IUnitOfWorkProvider unitOfWorkProvider, IUserService userService) : base(unitOfWorkProvider)
        {
            this.userService = userService;
        }
        
        public async Task<UserDto> GetUserAccordingToUsernameAsync(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userDto = await userService.GetUserAccordingToUsernameAsync(username);
                return userDto;
            }
        }
        
        public async Task<Guid> RegisterUserAsync(UserCreateDto userDtoInput)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userDto = await userService.RegisterUserAsync(userDtoInput);
                return userDto;
            }
        }
        
        public async Task<(bool success, Role role)> AuthorizeUserAsync(string username, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                var tuple = await userService.AuthorizeUserAsync(username, password);
                return tuple;
            }
        }
    }
}