using FaceShuffle.Application.PipelineBehaviors;
using MediatR;

namespace FaceShuffle.Application.Commands;
public class CreateSessionRequest : IRequest<CreateSessionResponse>, IUnitOfWorkRequest
{
    public required string Name { get; init; }
}
