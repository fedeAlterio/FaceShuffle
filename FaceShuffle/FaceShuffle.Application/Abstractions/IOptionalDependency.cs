using Optional;

namespace FaceShuffle.Application.Abstractions;
public interface IOptionalDependency<T>
{
    public Option<T> Optional { get; }
}
