using FaceShuffle.Models.Messages;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Repositories;
public interface IMessagesRepository
{
    DbSet<Message> DbSet { get; }
}
