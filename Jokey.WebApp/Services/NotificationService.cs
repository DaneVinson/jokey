namespace Jokey.WebApp.Services;

public class NotificationService : INotificationService
{
	private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly HubConnection _hubConnection;

	public event Action<Notification> OnNotificationReceived = default!;

	public NotificationService(
		NavigationManager navigationManager,
		AuthenticationStateProvider authenticationStateProvider)
	{
		_authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
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
		if (_hubConnection.State == HubConnectionState.Disconnected)
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			if (authState?.User.Identity?.IsAuthenticated ?? false)
			{
				try
				{
					await _hubConnection.StartAsync();
				}
				catch (HttpRequestException)
				{ }
			}
		}
	}
}
