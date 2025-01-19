namespace Jokey.Domain;

public sealed class AppOptions
{
    public string JokeUri { get; set; } = string.Empty;
    public string Joke2Uri { get; set; } = string.Empty;
    public string RenderTier { get; set; } = "?";
    public string UriDescription { get; set; } = string.Empty;
    public string Uri2Description { get; set; } = string.Empty;
}
