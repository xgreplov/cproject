using DemoEshop.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoEshop.DataAccessLayer.EntityFramework.Entities
{
    public class User : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(DemoEshopDbContext.Users);

        [Required]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// String with , delimiter.
        /// For example: "Admin,Editor,Tutor"
        /// </summary>
        public string Roles { get; set; }
    }
}
