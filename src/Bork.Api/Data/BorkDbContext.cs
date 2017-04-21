using Bork.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bork.Api.Data
{
    public class BorkDbContext : DbContext
    {
        public BorkDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public virtual DbSet<BorkRecord> Borks { get; set; }
        public virtual DbSet<ReBorkRecord> ReBorks { get; set; }
    }
}