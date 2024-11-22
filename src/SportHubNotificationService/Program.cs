using Hangfire;
using SportHubNotificationService;
using SportHubNotificationService.Api.Extensions;
using SportHubNotificationService.Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApp(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHangfireServer();

app.MapEndpoints();

app.Run();
