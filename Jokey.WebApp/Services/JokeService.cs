namespace Jokey.WebApp.Services;

public class JokeService : IJokeService
{
	private readonly HttpClient _httpClient;
	private readonly AppOptions _options;

	public JokeService(HttpClient httpClient, AppOptions options)
    {
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

	public async Task<string> GetJokeAsync()
    {
		var response = await _httpClient.GetAsync(_options.JokeUri);
		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}

		return $"The joke broke ({response.StatusCode} {response.ReasonPhrase}).";
    }
}
