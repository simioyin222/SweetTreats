using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PierresSweetAndSavoryTreats.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Treat> Treats { get; set; }
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<TreatFlavor> TreatFlavors { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<TreatFlavor>()
        .HasKey(tf => new { tf.TreatId, tf.FlavorId });

      builder.Entity<TreatFlavor>()
        .HasOne(tf => tf.Treat)
        .WithMany(t => t.Flavors)
        .HasForeignKey(tf => tf.TreatId);

      builder.Entity<TreatFlavor>()
        .HasOne(tf => tf.Flavor)
        .WithMany(f => f.Treats)
        .HasForeignKey(tf => tf.FlavorId);
    }
  }
}