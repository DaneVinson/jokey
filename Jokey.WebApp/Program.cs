var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetConfigurationObject<AuthOptions>();

builder.Services
    .AddSingleton(builder.Configuration.GetConfigurationObject<AppOptions>())
    .AddSingleton(authOptions)
    .AddSingleton<IJokeService, JokeService>()
    .AddScoped<IJokeService2, JokeService2>()
	.AddScoped<TokenHandler>()
	.AddHttpContextAccessor()
	.AddCascadingAuthenticationState()
	.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
	.AddAuthenticationStateSerialization();

builder.Services
    .AddAuth0WebAppAuthentication(options =>
    {
        options.ClientId = authOptions.ClientId;
        options.ClientSecret = authOptions.ClientSecret;
        options.Domain = authOptions.Domain;
    })
    .WithAccessToken(options =>
    {
        options.Audience = authOptions.Audience;
    });

builder.Services.AddHttpClient<JokeService>(nameof(JokeService));
builder.Services
    .AddHttpClient<JokeService2>(nameof(JokeService2))
    .AddHttpMessageHandler<TokenHandler>();

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

app
	.MapGet("/joke2", async (IJokeService2 jokeService) => await jokeService.GetJokeAsync())
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
