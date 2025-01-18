namespace Jokey.Domain;

public class JokeService2 : JokeService, IJokeService2
{
    public JokeService2(HttpClient httpClient, AppOptions options) : base(httpClient, options)
    {
        JokeUri = options.Joke2Uri;
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
