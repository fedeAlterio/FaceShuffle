using FaceShuffle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceShuffle.Infrastructure.Persistence.Configurations;
internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Name)
            .HasMaxLength(UserSession.NameMaximumLength);

        builder.HasIndex(x => x.Name)
            .IsClustered()
            .IsUnique();
    }
}
