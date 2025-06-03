using SportHubNotificationService.Api.Enpoints;
using SportHubNotificationService.Domain.Models;
using SportHubNotificationService.Infrastructure.Services;
using SportHubNotificationService.Validators;

namespace SportHubNotificationService.Features;

/// <summary>
/// Подписаться на события отправки уведомлений
/// </summary>
public class SendEmailNotificationsAboutUpdate
{
    private record SendEmailNotificationsAboutUpdateRequest(
        string[] Receivers,
        string Subject,
        string Body);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("email-notification-about-update", Handler);
        }
    }

    /// <summary>
    /// Обработчик отправления уведомлений об обновленном ЕКП файле
    /// </summary>
    /// <param name="request">Принимаемый запрос</param>
    /// <param name="service">Сервис отправки почтовых сообщений</param>
    /// <param name="validator">Сервис-валидатор email-адресов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    private static async Task<IResult> Handler(
        SendEmailNotificationsAboutUpdateRequest request,
        MailSenderService service,
        EmailValidator validator,
        CancellationToken cancellationToken = default)
    {
        var receivers = request.Receivers.ToList();

        var validationResult = validator.Execute(receivers);
        if (validationResult.IsFailure)
            return Results.BadRequest(validationResult.Error);

        var mailData = new MailData(receivers, request.Subject, request.Body);

        await service.Send(mailData);

        return Results.Ok();
    }
}