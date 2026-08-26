using HappiAdventure.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace HappiAdventure.Application.Db;

public class HappiAdventureDbContext(DbContextOptions<HappiAdventureDbContext> options) : DbContext(options)
{
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HappiAdventureDbContext).Assembly);
}
