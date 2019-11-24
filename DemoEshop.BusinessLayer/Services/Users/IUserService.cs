using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;
using System;
using System.Threading.Tasks;

namespace DemoEshop.BusinessLayer.Services.Users
{
    public interface IUserService
    {
        Task<Guid> RegisterUserAsync(UserCreateDto user);
        Task<(bool success, Role role)> AuthorizeUserAsync(string username, string password);
        Task<UserDto> GetUserAccordingToUsernameAsync(string username);
        
        Guid Create(UserDto entityDto);

        Task Update(UserDto entityDto);

        void Delete(Guid entityId);
    }
}
