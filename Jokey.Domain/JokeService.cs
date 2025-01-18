namespace Jokey.Domain;

public class JokeService : IJokeService
{
    public JokeService(HttpClient httpClient, AppOptions options)
    {
		HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		if (options?.JokeUri is null)
		{
			throw new ArgumentNullException(nameof(options.JokeUri));
		}
    
		JokeUri = options.JokeUri;
    }

	protected HttpClient HttpClient { get; private set; }
    protected string JokeUri { get; set; }

    public async Task<string> GetJokeAsync()
    {
		var response = await HttpClient.GetAsync(JokeUri);
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}

		return $"The joke broke ({response.StatusCode} {response.ReasonPhrase}).";
    }
}
