namespace Jokey.WebApp.Services;

public class UserService
{
	private ClaimsPrincipal CurrentUser { get; set; } = new(new ClaimsIdentity());

	public ClaimsPrincipal GetUser()
	{
		return CurrentUser;
	}

	internal void SetUser(ClaimsPrincipal user)
	{
		CurrentUser = user;
	}
}
