namespace Jokey.WebApp;

public sealed class AuthOptions
{
	public string Audience { get; set; } = string.Empty;
	public string ClientId { get; set; } = string.Empty;
	public string ClientSecret { get; set; } = string.Empty;
	public string Domain {  get; set; } = string.Empty;
}
