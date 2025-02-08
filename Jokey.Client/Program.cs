var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddSingleton(builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>()!)
    .AddSingleton(builder.Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>()!)
    .AddScoped<IJokeService, JokeService>()
    .AddScoped<IJokeService2, JokeService2>()
    .AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient<JokeService>("JokeyWebApp");
builder.Services.AddHttpClient<JokeService2>("JokeyApi");

await builder.Build().RunAsync();
