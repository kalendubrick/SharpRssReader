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

app.MapGet("/feed", async (string feedUrl, [FromServices] IFeedRetrievalService feedService) =>
{
    try
    {
        var feed = await feedService.ReadFeedAsync(feedUrl);

        return Results.Ok(feed);
    }
    catch (FeedRequestException e)
    {
        return Results.BadRequest(e.Message);
    }
    catch (FeedEmptyException e)
    {
        return Results.BadRequest(e.Message);
    }
    catch (FeedLoadException)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
})
.Produces<SyndicationFeed>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest)
.ProducesProblem(StatusCodes.Status500InternalServerError)
.WithName("GetFeed").WithTags("Getters");

app.Run();
