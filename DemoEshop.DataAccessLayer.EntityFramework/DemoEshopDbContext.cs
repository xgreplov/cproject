using System.Data.Common;
using System.Data.Entity;
using DemoEshop.DataAccessLayer.EntityFramework.Config;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;

namespace DemoEshop.DataAccessLayer.EntityFramework
{
    public class DemoEshopDbContext : DbContext
    {
        /// <summary>
        /// Non-parametric ctor used by data access layer
        /// </summary>
        public DemoEshopDbContext() : base(EntityFrameworkInstaller.ConnectionString)
        {
            // force load of EntityFramework.SqlServer.dll into build
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        /// <summary>
        /// Ctor with db connection, required by data access layer tests
        /// </summary>
        /// <param name="connection">The database connection</param>
        public DemoEshopDbContext(DbConnection connection) : base(connection, true)
        {
            Database.CreateIfNotExists();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RateSong> RateSongs { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
