using DemoEshop.BusinessLayer.DataTransferObjects;
using System;
using System.Threading.Tasks;

namespace DemoEshop.BusinessLayer.Services.Users
{
    public interface IUserService
    {
        Task<Guid> RegisterUserAsync(UserCreateDto user);
        Task<(bool success, string roles)> AuthorizeUserAsync(string username, string password);
        Task<UserDto> GetUserAccordingToUsernameAsync(string username);
    }
}
