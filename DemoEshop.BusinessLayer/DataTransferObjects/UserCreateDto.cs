using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    class UserCreateDto : DtoBase
    {
        [Required(ErrorMessage = "Username is required!")]
        [MaxLength(32, ErrorMessage = "Your username is too long!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "This is not valid email address!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 30")]
        public string Password { get; set; }
    }
}
