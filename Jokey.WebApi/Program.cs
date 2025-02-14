var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetConfigurationObject<AuthOptions>();

builder.Services
	.AddSingleton(authOptions)
    .AddSingleton(builder.Configuration.GetConfigurationObject<AppOptions>())
    .AddSingleton<IJokeService, JokeService>();

builder.Services.AddHttpClient<JokeService>(nameof(JokeService));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.Authority = authOptions.Authority;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidAudience = authOptions.Audience,
			ValidIssuer = authOptions.Domain,
			ValidateLifetime = true
		};
	});

builder.Services.AddAuthorization();

var app = builder.Build();

app
	.UseCors("AllowAll")
	.UseHttpsRedirection()
	.UseAuthentication()
	.UseAuthorization();

app
	.MapGet("/joke", async (IJokeService jokeService) => await jokeService.GetJokeAsync())
	.WithName("joke")
	.RequireAuthorization();

app.Run();
