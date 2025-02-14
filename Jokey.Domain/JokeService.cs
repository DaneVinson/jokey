namespace Jokey.Domain;

public sealed class JokeService : IJokeService
{
	private readonly HttpClient _httpClient;
	private readonly string _jokeUri;

	public JokeService(IHttpClientFactory httpClientFactory, AppOptions options)
	{
		_httpClient = httpClientFactory?.CreateClient(nameof(JokeService)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
		if (options?.JokeUri is null)
		{
			throw new ArgumentNullException(nameof(options.JokeUri));
		}

		_jokeUri = options.JokeUri;
	}

	public async Task<string> GetJokeAsync()
	{
		var response = await _httpClient.GetAsync(_jokeUri);
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}

		return $"The joke broke ({response.StatusCode:D} {response.ReasonPhrase}).";
	}
}
