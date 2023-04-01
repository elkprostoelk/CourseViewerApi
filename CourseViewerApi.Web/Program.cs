using Microsoft.AspNetCore.Diagnostics;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuiderContext, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(hostBuiderContext.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Logger;
app.UseExceptionHandler(appBuilder => appBuilder.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature.Error;
    logger.LogCritical(exception, "An exception occured while processing the request");
    context.Response.StatusCode = 500;
    await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error"});
}));

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();