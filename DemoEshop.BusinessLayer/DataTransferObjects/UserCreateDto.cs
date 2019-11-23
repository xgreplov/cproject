﻿using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace DemoEshop.BusinessLayer.DataTransferObjects
{
    public class UserCreateDto : DtoBase
    {
        [Required(ErrorMessage = "First name is required!")]
        [MaxLength(64, ErrorMessage = "Your first name is too long!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "This is not valid email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone]
        public string MobilePhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Address is required!")]
        [MaxLength(1024)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Birthdate is required!")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 30")]
        public string Password { get; set; }
    }
}