using System.Text;
using Hangfire;
using Newtonsoft.Json;
using SportHubNotificationService.Domain.Models;

namespace SportHubNotificationService.Jobs;

public class RepeatSendingCheckPDFJob(
    IHttpClientFactory clientFactory,
    ILogger<SendToTelegramRequestJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute()
    {
        try
        {
            using var client = clientFactory.CreateClient();

            var route = $"http://localhost:4014/run-parser";
            
            using var request = new HttpRequestMessage(HttpMethod.Post, route);
            
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Response: {responseData}");
            }
            else
            {
                logger.LogError($"Error: {response.StatusCode}");
            }
            
            logger.LogInformation("request sent to parser");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot send request, ex: {ex}", ex.Message);
        }
    }
}