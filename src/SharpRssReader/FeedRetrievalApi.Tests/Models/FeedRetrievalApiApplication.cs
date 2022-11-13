using FeedRetrievalApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeedRetrievalApi.Tests.Models;

internal class FeedRetrievalApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IFeedRetrievalService, FeedRetrievalService>();
        });

        return base.CreateHost(builder);
    }
}
