using System.Reflection;
using FaceShuffle.Models.Messages;
using FaceShuffle.Models.PendingJobs;
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
    public DbSet<PendingJob> PendingJobs { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
}
