using Serilog;
using SportHubNotificationService.Api.Extensions;
using SportHubNotificationService.Api.Middlewares;
using SportHubNotificationService.Application.Validators;
using SportHubNotificationService.Infrastructure.Services;
using SportHubNotificationService.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MailOptions>(
    builder.Configuration.GetSection(MailOptions.SECTION_NAME));
builder.Services.AddScoped<EmailValidator>();
builder.Services.AddScoped<MailSenderService>();

builder.Services.AddLogger(builder.Configuration);
builder.Services.AddSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.Run();
