using SketchSync.Components;
using SketchSync.Hubs;
using SketchSync.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SignalR
builder.Services.AddSignalR();

// Add custom services
builder.Services.AddSingleton<RoomService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

// Map SignalR hub
app.MapHub<SketchHub>("/sketchhub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
