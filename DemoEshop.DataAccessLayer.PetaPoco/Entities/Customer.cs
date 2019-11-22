using System;
using AsyncPoco;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    /// <summary>
    /// Represents eshop customer
    /// </summary>
    [TableName(TableNames.UserTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Customer : User
    {
        public Customer()
        {
            Discriminator = nameof(Customer);
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime BirthDate { get; set; } = new DateTime(1950, 1, 1);
    }
}