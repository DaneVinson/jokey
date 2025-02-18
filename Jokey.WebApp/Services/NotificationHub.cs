namespace Jokey.WebApp.Services;

public sealed class NotificationHub : Hub
{
    public const string ClientReceiveMethodName = "ReceiveNotification";

	public async Task SendNotification(Notification notification)
    {
        await Clients.All.SendAsync(ClientReceiveMethodName, notification);
    }

	public override async Task OnConnectedAsync()
	{	
		var group = GetGroupName();
		if (group == string.Empty)
		{
			return;
		}

		await Groups.AddToGroupAsync(Context.ConnectionId, group);
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		var group = GetGroupName();
		if (group == string.Empty)
		{
			return;
		}

		await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
		await base.OnDisconnectedAsync(exception);
	}

	private string GetGroupName() =>
		Context
			.User?
			.Identities
			.FirstOrDefault()?
			.Claims?
			.FirstOrDefault(c => c.Type is ClaimTypes.Name or "name")?
			.Value.ToLower() ?? 
			string.Empty;
}
