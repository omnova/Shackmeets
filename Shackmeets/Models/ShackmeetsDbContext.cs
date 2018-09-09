using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets.Models
{
  public class ShackmeetsDbContext : DbContext
  {
    public ShackmeetsDbContext(DbContextOptions<ShackmeetsDbContext> options)
      : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
        .HasKey(u => u.Username);

      modelBuilder.Entity<User>()
        .HasMany(u => u.Rsvps)
        .WithOne(r => r.User);

      modelBuilder.Entity<User>()
        .HasMany(u => u.Meets)
        .WithOne(m => m.Organizer);

      //modelBuilder.Entity<Meet>()
      //  .HasOne(m => m.Organizer)
      //  .WithMany(u => u.Meets);

      modelBuilder.Entity<Meet>()
        .HasMany(m => m.Rsvps)
        .WithOne(r => r.Meet);

      //modelBuilder.Entity<Post>()
      //      .HasOne(p => p.Blog)
      //      .WithMany(b => b.Posts)
      //      .HasForeignKey(p => p.BlogUrl)
      //      .HasPrincipalKey(b => b.Url);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Meet> Meets { get; set; }
    public DbSet<Rsvp> Rsvps { get; set; }
  }
}
