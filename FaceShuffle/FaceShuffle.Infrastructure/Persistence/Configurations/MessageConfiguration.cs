using FaceShuffle.Models.Messages;
using FaceShuffle.Models.Messages.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceShuffle.Infrastructure.Persistence.Configurations;
internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x))
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MessageTextContent)
            .HasConversion(x => x.Value, x => new(x))
            .HasMaxLength(MessageTextContentValidator.MaximumLength);

        builder.Property(x => x.Receiver)
            .HasConversion(UserSessionConfiguration.SessionGuidConverter);

        builder.Property(x => x.Sender)
            .HasConversion(UserSessionConfiguration.SessionGuidConverter);
    }
}
