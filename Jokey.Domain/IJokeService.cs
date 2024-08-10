namespace Jokey.Domain;

public interface IJokeService
{
    Task<string> GetJokeAsync();
}
