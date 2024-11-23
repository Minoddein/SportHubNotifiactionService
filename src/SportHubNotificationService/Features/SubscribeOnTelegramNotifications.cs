using Hangfire;
using SportHubNotificationService.Api.Enpoints;
using SportHubNotificationService.Infrastructure.Services;
using SportHubNotificationService.Jobs;
using SportHubNotificationService.Validators;

namespace SportHubNotificationService.Features;

public class SubscribeOnTelegramNotifications
{
    private record SubscribeOnTelegramNotificationsRequest(
        long ChatId, 
        DateTime CompetitionDate,
        string Message);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("telegram-notification", Handler);
        }
    }
    
    /// <summary>
    /// Обработчик отправления уведомлений с фильтрацией
    /// </summary>
    /// <param name="request">Принимаемый запрос</param>
    /// <param name="service">Сервис отправки почтовых сообщений</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    private static async Task<IResult> Handler( 
        SubscribeOnTelegramNotificationsRequest request,
        TelegramBotHttpClient service,
        CancellationToken cancellationToken = default)
    {
        List<long> chatIds = [request.ChatId];
        
        //TODO: Для теста в минутах: через 1,2,3
        // За месяц до соревнований
        var oneMonthBefore = request.CompetitionDate.AddMinutes(1);
        BackgroundJob.Schedule<SendToTelegramRequestJob>(job => 
            job.Execute(chatIds, request.Message), oneMonthBefore);

        // За неделю до соревнований
        var oneWeekBefore = request.CompetitionDate.AddMinutes(2);
        BackgroundJob.Schedule<SendToTelegramRequestJob>(job => 
            job.Execute(chatIds, request.Message), oneWeekBefore);
        
        // За два дня до соревнований
        var twoDaysBefore = request.CompetitionDate.AddMinutes(3);
        BackgroundJob.Schedule<SendToTelegramRequestJob>(job => 
            job.Execute(chatIds, request.Message), twoDaysBefore);
        
        return Results.Ok();
    }
}