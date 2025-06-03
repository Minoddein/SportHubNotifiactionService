using Hangfire;
using Microsoft.Extensions.Options;
using SportHubNotificationService.Options;

namespace SportHubNotificationService.Jobs;

public class RepeatSendingCheckPDFJob(
    IOptions<ParserOptions> parserOptions,
    IHttpClientFactory clientFactory,
    ILogger<SendToTelegramRequestJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute()
    {
        try
        {
            using var client = clientFactory.CreateClient();

            var route = parserOptions.Value.ParserURL;

            using var request = new HttpRequestMessage(HttpMethod.Post, route);

            await client.SendAsync(request);

            logger.LogInformation("request sent to parser");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot send request, ex: {ex}", ex.Message);
        }
    }
}