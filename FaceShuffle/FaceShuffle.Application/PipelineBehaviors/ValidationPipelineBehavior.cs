using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Extensions;
using FluentValidation;
using MediatR;
using Optional;

namespace FaceShuffle.Application.PipelineBehaviors;
public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Option<IValidator<TRequest>> _validatorOptional;

    public ValidationPipelineBehavior(IOptionalDependency<IValidator<TRequest>> validatorOptional)
    {
        _validatorOptional = validatorOptional.Optional;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validatorOptional.TryGetValue(out var validator))
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
        }

        return await next();
    }
}
