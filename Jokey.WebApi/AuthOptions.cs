namespace Jokey.WebApi;

public class AuthOptions
{
	public string Audience { get; set; } = string.Empty;
	public string Authority => $"https://{Domain}/";
	public string Domain { get; set; } = string.Empty;
}
