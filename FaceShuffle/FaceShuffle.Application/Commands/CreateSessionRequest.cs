using MediatR;

namespace FaceShuffle.Application.Commands;
public class CreateSessionRequest : IRequest<CreateSessionResponse>
{
    public required string Name { get; init; }
}
