using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DailyTodo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.SetMinimumLevel(LogLevel.Error);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add appsettings.json to configuration sources
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Register AppConfig
builder.Services.AddSingleton(sp =>
{
    var appConfig = new AppConfig
    {
        DailyTasks = builder.Configuration.GetSection("DailyTasks").Get<List<string>>() ?? new List<string>()
    };
    return appConfig;
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
