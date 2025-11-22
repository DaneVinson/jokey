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
		if (!(Context.User?.Identity?.IsAuthenticated ?? true))
		{
			return;
		}

		await Groups.AddToGroupAsync(Context.ConnectionId, Context.User!.Identity!.Name!);

		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		if (!(Context.User?.Identity?.IsAuthenticated ?? true))
		{
			return;
		}

		await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User!.Identity!.Name!);

		await base.OnDisconnectedAsync(exception);
	}
}
