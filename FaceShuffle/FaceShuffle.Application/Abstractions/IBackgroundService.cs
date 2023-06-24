namespace FaceShuffle.Application.Abstractions;
public interface IBackgroundService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
  