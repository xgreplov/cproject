using System.ComponentModel.DataAnnotations;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Enums;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class UserDto : DtoBase
    {
        [Required, MaxLength(32)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public string Email { get; set; }
    }
}
