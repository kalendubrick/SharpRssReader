using FeedRetrievalApi.Exceptions;
using FeedRetrievalApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IFeedRetrievalService, FeedRetrievalService>();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/feed", async ([FromBody] string feedUrl, [FromServices] FeedRetrievalService feedService, HttpResponse response) =>
{
    try
    {
        var feed = await feedService.ReadFeedAsync(feedUrl);

        response.StatusCode = StatusCodes.Status200OK;
        await response.WriteAsJsonAsync(feed);
    }
    catch (FeedRequestException e)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        await response.WriteAsJsonAsync(e.Message);
    }
    catch (FeedEmptyException e)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        await response.WriteAsJsonAsync(e.Message);
    }
    catch (FeedLoadException e)
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await response.WriteAsJsonAsync(e.Message);
    }
})
.Produces<SyndicationFeed>(StatusCodes.Status200OK)
.WithName("GetFeed").WithTags("Getters");

app.Run();
