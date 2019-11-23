using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class UserDto : DtoBase
    {
        public Guid Id { get; set; }

        public string TableName { get; } = nameof(DemoEshopDbContext.Users);

        public string Username { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public string Email { get; set; }
    }
}
