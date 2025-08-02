namespace Jokey.WebApp.Services;

/// <summary>
/// Simple user context implementation to store the current user's name and provide thread-safe access.
/// </summary>
public class UserContext : IUserContext
{
	private readonly Lock _lock = new();

	public Guid Id { get; } = Guid.NewGuid();
	public string UserName { get; private set; } = string.Empty;

	public void SetUserName(string userName)
	{
		using var scope = _lock.EnterScope();
		UserName = userName ?? string.Empty;
	}
}
