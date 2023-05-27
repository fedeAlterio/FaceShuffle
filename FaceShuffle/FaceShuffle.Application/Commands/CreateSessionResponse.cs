using FaceShuffle.Models;

namespace FaceShuffle.Application.Commands;
public class CreateSessionResponse
{
    public required UserSession UserSession { get; init; }
    public required string JsonWebToken { get; set; }
}
