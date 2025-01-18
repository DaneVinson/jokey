var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(new AppOptions()
    {
        JokeUri = Jokes.JokeApi_Pun
    })
    .AddSingleton<IJokeService, JokeService>();

builder.Services.AddHttpClient<JokeService>("jokeapi.dev");

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
