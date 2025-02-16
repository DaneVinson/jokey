namespace Jokey.WebApp.Services;

public class NotificationService : INotificationService
{
	private readonly HubConnection _hubConnection;

	public event Action<Notification> OnNotificationReceived = default!;

	public NotificationService(NavigationManager navigationManager)
	{
		_hubConnection = new HubConnectionBuilder()
								.WithUrl(navigationManager.ToAbsoluteUri("/notifications"))
								.WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)])
								.Build();

		_hubConnection.On<Notification>("ReceiveNotification", (notification) =>
		{
			OnNotificationReceived?.Invoke(notification);
		});
	}

	public async Task StartAsync()
	{
		await _hubConnection.StartAsync();
	}
}
