using System;
using System.ComponentModel.DataAnnotations;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    /// <summary>
    /// Represents eshop customer
    /// </summary>
    public class Customer : User
    {
        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string MobilePhoneNumber { get; set; }

        [MaxLength(1024)]
        public string Address { get; set; }

        public DateTime BirthDate { get; set; } = new DateTime(1950, 1, 1);
    }
}