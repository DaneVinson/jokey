var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddSingleton(new AppOptions()
    {
        JokeUri = $"{builder.HostEnvironment.BaseAddress}joke",
        Joke2Uri = "https://localhost:7192/joke",
        RenderTier = "Client",
        UriDescription = "Hosting API",
        Uri2Description = "Remote Jokey API"
    })
    .AddScoped<IJokeService, JokeService>()
    .AddScoped<IJokeService2, JokeService2>();

builder.Services.AddHttpClient<JokeService>("JokeyWebApp");
builder.Services.AddHttpClient<JokeService2>("JokeyApi");

await builder.Build().RunAsync();
