namespace SportHubNotificationService.Application.Abstraction;

public interface IMigrator
{
    Task Migrate(CancellationToken cancellationToken = default);
}