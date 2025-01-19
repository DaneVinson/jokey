var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>()!)
    .AddSingleton(builder.Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>()!)
    .AddSingleton<IJokeService, JokeService>()
    .AddSingleton<IJokeService2, JokeService2>()
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient<JokeService>("jokeapi.dev");
builder.Services.AddHttpClient<JokeService2>("JokeyApi");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app
        .UseExceptionHandler("/Error", createScopeForErrors: true)
        .UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Jokey.Client._Imports).Assembly);

app.MapGet("/joke", async (IJokeService jokeService) =>
{
	return await jokeService.GetJokeAsync();
});

app.Run();
