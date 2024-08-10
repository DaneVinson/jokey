namespace Jokey.Client.Services;

internal class HttpJokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    private readonly AppOptions _options;

    public HttpJokeService(HttpClient httpClient, AppOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        _httpClient.BaseAddress = new Uri(options.JokeUri);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetJokeAsync()
    {
        var response = await _httpClient.GetAsync("/joke");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return $"Joke's on you. This shit broke ({response.StatusCode} {response.ReasonPhrase}).";
    }
}
