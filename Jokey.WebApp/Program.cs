var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>() ??
                    throw new NullReferenceException($"Could not bind configuration to {nameof(AuthOptions)}");
var appOptions = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>() ??
                    throw new NullReferenceException($"Counld not bind configuration to {nameof(AppOptions)}");

builder.Services
    .AddSingleton(appOptions)
    .AddSingleton(authOptions)
    .AddSingleton<IJokeService, JokeService>()
    .AddSingleton<IJokeService2, JokeService2>()
	.AddCascadingAuthenticationState()
	.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
	.AddAuthenticationStateSerialization();

builder.Services
    .AddAuth0WebAppAuthentication(options => 
    {
		options.ClientId = authOptions.ClientId;
		options.Domain = authOptions.Domain;
    });

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

app
    .MapGet("/joke", async (IJokeService jokeService) => await jokeService.GetJokeAsync())
    .RequireAuthorization();

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
	var authProperties = new LoginAuthenticationPropertiesBuilder()
			                    .WithRedirectUri(returnUrl)
			                    .Build();

	await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
	var authProperties = new LogoutAuthenticationPropertiesBuilder()
			                    .WithRedirectUri("/")
			                    .Build();

	await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authProperties);
	await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.Run();
