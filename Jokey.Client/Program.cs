using Jokey.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddSingleton(builder.Configuration.GetConfigurationObject<AppOptions>())
    .AddScoped<IJokeService, JokeService>()
    .AddScoped<IJokeService2, JokeService2>()
    .AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient<JokeService>("JokeyWebApp");
builder.Services.AddHttpClient<JokeService2>("JokeyApi");

await builder.Build().RunAsync();
