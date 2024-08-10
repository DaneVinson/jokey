using Jokey.Client;
using Jokey.Client.Services;
using Microsoft.AspNetCore.Components.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddSingleton(new AppOptions() 
	{ 
		JokeUri = builder.HostEnvironment.BaseAddress,
		RenderTier = "Client"
	})
	.AddScoped<IJokeService, HttpJokeService>()
	.AddHttpClient<HttpJokeService>("JokeClient");

await builder.Build().RunAsync();
