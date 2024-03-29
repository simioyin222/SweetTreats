using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PierresSweetAndSavoryTreats.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Treat> Treats { get; set; }
    public DbSet<Flavor> Flavors { get; set; }

    public DbSet<TreatFlavor> TreatFlavors { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) { }
  }
}