var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddSingleton(builder.Configuration.GetConfigurationObject<AppOptions>())
	.AddScoped<IJokeService, JokeService>()
    .AddScoped<IJokeService2, JokeService2>()
	.AddBlazoredToast()
	.AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient<JokeService>(nameof(JokeService));
builder.Services.AddHttpClient<JokeService2>(nameof(JokeService2));

await builder.Build().RunAsync();
