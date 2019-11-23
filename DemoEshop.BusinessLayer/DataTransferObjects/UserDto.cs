using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class UserDto : DtoBase
    {
        public string Username { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public string Email { get; set; }
    }
}
