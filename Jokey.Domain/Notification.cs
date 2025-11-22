namespace Jokey.Domain;

public sealed class Notification
{
	public Notification()
	{ }

	public Notification(string userName, string message)
	{
		UserName = userName;
		Message = message;
	}

	public string Message { get; set; } = string.Empty;	
	public string UserName { get; set; } = string.Empty;
}
