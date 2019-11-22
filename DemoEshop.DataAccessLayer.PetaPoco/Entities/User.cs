using System;
using AsyncPoco;
using DemoEshop.Infrastructure;

namespace DemoEshop.DataAccessLayer.PetaPoco.Entities
{
    [TableName(TableNames.UserTable)]
    [PrimaryKey("Id", autoIncrement = false)]
    public class User : IEntity
    {
        public User()
        {
            if (string.IsNullOrEmpty(Discriminator))
            {
                Discriminator = nameof(User);
            }
        }

        public Guid Id { get; set; }

        [Ignore]
        public string TableName { get; } = TableNames.UserTable;
        
        public string Username { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public string PasswordHash { get; set; }

        /// <summary>
        /// String with , delimiter.
        /// For example: "Admin,Editor,Tutor"
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Helper property determining the type 
        /// of the entity stored in table hierarchy.
        /// </summary>
        public string Discriminator { get; set; }
    }
}
