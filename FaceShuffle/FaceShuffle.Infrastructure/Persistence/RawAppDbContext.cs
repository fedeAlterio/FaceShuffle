using System.Reflection;
using FaceShuffle.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Infrastructure.Persistence;
public class RawAppDbContext : DbContext
{
    public RawAppDbContext(DbContextOptions<RawAppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<UserSession>().ToTable(nameof(UserSessions));
    }

    public DbSet<UserSession> UserSessions { get; set; } = default!;
}
