using Hangfire;
using SportHubNotificationService;
using SportHubNotificationService.Api.Extensions;
using SportHubNotificationService.Api.Middlewares;
using SportHubNotificationService.Jobs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApp(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddEndpoints();

var app = builder.Build();

app.UseExceptionMiddleware();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}*/

app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard();

app.UseCors(c =>
    c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard();

app.UseHangfireServer();

app.MapEndpoints();

RecurringJob.AddOrUpdate<RepeatSendingCheckPDFJob>(
    j => j.Execute(), "0 */6 * * *");

app.Run();

