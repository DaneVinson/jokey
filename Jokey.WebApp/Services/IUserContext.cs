namespace Jokey.WebApp.Services;

public interface IUserContext
{
	Guid Id { get; }
	string UserName { get; }

	void SetUserName(string userName);
}
