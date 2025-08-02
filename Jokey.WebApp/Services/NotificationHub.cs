namespace Jokey.WebApp.Services;

public sealed class NotificationHub : Hub
{
    public const string ClientReceiveMethodName = "ReceiveNotification";

	private readonly IUserContext _userContext;

	public NotificationHub(IUserContext userContext)
	{
		_userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
	}

	public async Task SendNotification(Notification notification)
    {
        await Clients.All.SendAsync(ClientReceiveMethodName, notification);
    }

	public override async Task OnConnectedAsync()
	{

		var c = Context;

		if (string.IsNullOrEmpty(_userContext.UserName))
		{
			return;
		}

		await Groups.AddToGroupAsync(Context.ConnectionId, _userContext.UserName);
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		if (string.IsNullOrEmpty(_userContext.UserName))
		{
			return;
		}

		await Groups.RemoveFromGroupAsync(Context.ConnectionId, _userContext.UserName);
		await base.OnDisconnectedAsync(exception);
	}
}
