using SportHubNotificationService.Api.Enpoints;
using SportHubNotificationService.Domain.Models;
using SportHubNotificationService.Infrastructure.Services;

namespace SportHubNotificationService.Features;

public class SendEmailNotificationByFilter
{
    private record SendEmailNotificationByFilterRequest(IEnumerable<string> Recievers, string Subject, string Body);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("sending", Handler);
        }
    }
    private static async Task<IResult> Handler( 
        SendEmailNotificationByFilterRequest request,
        MailSenderService service,
        CancellationToken cancellationToken = default)
    {
        var mailData = new MailData(request.Recievers, request.Subject, request.Body);
        
        var result = await service.Send(mailData);
        
        return Results.Ok();
    }
}