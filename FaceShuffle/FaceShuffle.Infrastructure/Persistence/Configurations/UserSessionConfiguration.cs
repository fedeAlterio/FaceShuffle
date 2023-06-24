using FaceShuffle.Models.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceShuffle.Infrastructure.Persistence.Configurations;
internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasConversion(x => x.Value, x => new (x))
            .IsRequired();

        builder.HasIndex(p => p.Username)
            .IsUnique();
    }
}
