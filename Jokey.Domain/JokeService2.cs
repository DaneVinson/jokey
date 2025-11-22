namespace Jokey.Domain;

public class JokeService2 : IJokeService2
{
	private readonly HttpClient _httpClient;
	private readonly string _jokeUri;

	public JokeService2(IHttpClientFactory httpClientFactory, AppOptions options)
	{
		_httpClient = httpClientFactory?.CreateClient(nameof(JokeService2)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
		if (options?.Joke2Uri is null)
		{
			throw new ArgumentNullException(nameof(options.JokeUri));
		}

		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_jokeUri = options.Joke2Uri;
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
