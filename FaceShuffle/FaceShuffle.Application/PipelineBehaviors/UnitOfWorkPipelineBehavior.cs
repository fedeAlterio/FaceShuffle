using FaceShuffle.Application.Abstractions;
using MediatR;

namespace FaceShuffle.Application.PipelineBehaviors;
public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAppDbContext _appDbContext;
    private int _concurrentCalls;

    public UnitOfWorkPipelineBehavior(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is INotUnitOfWorkRequest)
        {
            return await next();
        }

        _concurrentCalls++;

        var ret = await next();

        _concurrentCalls--;
        if (_concurrentCalls == 0)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        return ret;
    }
}
