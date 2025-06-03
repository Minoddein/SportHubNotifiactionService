using Hangfire;
using SportHubNotificationService.Api.Enpoints;
using SportHubNotificationService.Infrastructure.Services;
using SportHubNotificationService.Jobs;
using SportHubNotificationService.Validators;

namespace SportHubNotificationService.Features;

/// <summary>
/// Подписаться на события отправки уведомлений
/// </summary>
public class SubscribeOnEmailNotifications
{
    private record SubscribeOnEmailNotificationsRequest(
        string Receiver, 
        DateTime CompetitionDate,
        string Subject,
        string Body);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("email-notification", Handler);
        }
    }
    
    /// <summary>
    /// Обработчик отправления уведомлений с фильтрацией
    /// </summary>
    /// <param name="request">Принимаемый запрос</param>
    /// <param name="service">Сервис отправки почтовых сообщений</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    private static Task<IResult> Handler( 
        SubscribeOnEmailNotificationsRequest request,
        MailSenderService service,
        EmailValidator validator,
        CancellationToken cancellationToken = default)
    {
        List<string> receivers = [request.Receiver];

        var validationResult = validator.Execute(receivers);
        if (validationResult.IsFailure)
            return Task.FromResult(Results.BadRequest(validationResult.Error));
        
        //TODO: Для теста в минутах: через 1,2,3
        
        // За месяц до соревнований
        var oneMonthBefore = request.CompetitionDate.AddMinutes(1);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(receivers, request.Subject, request.Body), oneMonthBefore);

        // За неделю до соревнований
        var oneWeekBefore = request.CompetitionDate.AddMinutes(2);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(receivers, request.Subject, request.Body), oneWeekBefore);
        
        // За два дня до соревнований
        var twoDaysBefore = request.CompetitionDate.AddMinutes(3);
        BackgroundJob.Schedule<SendEmailJob>(job => 
            job.Execute(receivers, request.Subject, request.Body), twoDaysBefore);
        
        return Task.FromResult(Results.Ok());
    }
}