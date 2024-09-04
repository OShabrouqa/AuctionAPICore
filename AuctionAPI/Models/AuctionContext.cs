using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AuctionAPI.Models
{
    public class AuctionContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<Bidder> Bidders { get; set; }

        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasMany(p => p.Bidders)
                .WithOne(b => b.Property)
                .HasForeignKey(b => b.PropertyId);
        }
    }

}
