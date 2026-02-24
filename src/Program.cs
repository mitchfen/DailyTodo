using DailyTodo;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<TodoService>();

// Register AppConfig
builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    
    // Check for comma-separated environment variable first (legacy support)
    var dailyTasksEnv = Environment.GetEnvironmentVariable("DAILY_TASKS");
    List<string> tasks;

    if (!string.IsNullOrWhiteSpace(dailyTasksEnv))
    {
        tasks = dailyTasksEnv.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Trim())
                            .ToList();
    }
    else
    {
        tasks = configuration.GetSection("DailyTasks").Get<List<string>>() ?? new List<string>();
    }

    return new AppConfig { DailyTasks = tasks };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
