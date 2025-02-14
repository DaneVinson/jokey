var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddSingleton(builder.Configuration.GetConfigurationObject<AppOptions>())
	.AddSingleton(builder.Configuration.GetConfigurationObject<AuthOptions>())
    .AddSingleton<IJokeService, JokeService>();

builder.Services.AddHttpClient<JokeService>(nameof(JokeService));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.MapGet("/joke", async (IJokeService jokeService) =>
{
    return await jokeService.GetJokeAsync();
});

app.Run();
