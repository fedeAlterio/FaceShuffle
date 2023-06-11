using FaceShuffle.Models;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Abstractions.Repositories;
public interface IUserSessionRepository
{
    DbSet<UserSession> DbSet { get; }
    Task<UserSession> GetActiveSessionByName(string name, CancellationToken cancellationToken);
}
