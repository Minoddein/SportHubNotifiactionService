using Hangfire;
using SportHubNotificationService.Api.Enpoints;
using SportHubNotificationService.Domain.Models;
using SportHubNotificationService.Infrastructure.Services;
using SportHubNotificationService.Jobs;

namespace SportHubNotificationService.Features;

public class SubscribeOnNotify
{
    private record SubscribeOnNotifyRequest(
        string Reciever, 
        DateTime CompetitionDate,
        string Subject,
        string Body);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("subscribing", Handler);
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
        SubscribeOnNotifyRequest request,
        MailSenderService service,
        CancellationToken cancellationToken = default)
    {
        List<string> recievers = [request.Reciever];
        
        // За месяц до соревнований
        var oneMonthBefore = request.CompetitionDate.AddMinutes(1);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(recievers, request.Subject, request.Body), oneMonthBefore);

        // За неделю до соревнований
        var oneWeekBefore = request.CompetitionDate.AddMinutes(2);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(recievers, request.Subject, request.Body), oneWeekBefore);
        
        // За два дня до соревнований
        var twoDaysBefore = request.CompetitionDate.AddMinutes(3);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(recievers, request.Subject, request.Body), twoDaysBefore);
        
        return Results.Ok();
    }
}