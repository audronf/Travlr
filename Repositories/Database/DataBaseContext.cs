#region Using
using Microsoft.EntityFrameworkCore;
using Funtrip.Models;
#endregion
 namespace Funtrip.Repositories.Database
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) {}
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}