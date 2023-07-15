using FaceShuffle.Application.Repositories;
using FaceShuffle.Models.Messages;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Infrastructure.Persistence.Repositories;
public class MessagesRepository : IMessagesRepository
{
    private readonly RawAppDbContext _appDbContext;

    public MessagesRepository(RawAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public DbSet<Message> DbSet => _appDbContext.Messages;
}
