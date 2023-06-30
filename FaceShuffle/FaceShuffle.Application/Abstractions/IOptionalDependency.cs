using FaceShuffle.Models.Generic;

namespace FaceShuffle.Application.Abstractions;
public interface IOptionalDependency<T>
{
    public Optional<T> Optional { get; }
}
