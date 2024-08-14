var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	//.AddSingleton<AppOptions>(new AppOptions() 
	//{
	//	JokeUri = builder.HostEnvironment.BaseAddress,
	//	RenderTier = "Client"
	//})
	.AddTransient<AppOptions>(provider => new AppOptions()
	{
		JokeUri = builder.HostEnvironment.BaseAddress,
		RenderTier = "Client"
	})
	.AddTransient<IJokeService, HttpJokeService>()
	.AddHttpClient<HttpJokeService>("JokeClient");

await builder.Build().RunAsync();
