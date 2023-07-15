using FaceShuffle.Models.Session;
using FaceShuffle.Models.Session.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FaceShuffle.Infrastructure.Persistence.Configurations;
internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public static ValueConverter<UserSessionGuid, Guid> SessionGuidConverter { get; } = new(x => x.Value, x => new(x));
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x))
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Bio)
            .HasConversion(x => x.Value, x => new(x))
            .HasMaxLength(BioValidator.MaximumLength);

        builder.Property(x => x.UserAge)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.UserFullName)
            .HasConversion(x => x.Value, x => new(x))
            .HasMaxLength(UserFullNameValidator.MaximumLength);

        builder.Property(x => x.Username)
            .HasConversion(x => x.Value, x => new (x))
            .HasMaxLength(UsernameValidator.MaximumLength)
            .IsRequired();

        builder.Property(x => x.SessionGuid)
            .HasConversion(SessionGuidConverter);

        builder.HasIndex(p => p.Username)
            .IsUnique();
    }
}
