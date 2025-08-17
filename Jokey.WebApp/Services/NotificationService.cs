namespace Jokey.WebApp.Services;

public class NotificationService : INotificationService, IAsyncDisposable
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly NavigationManager _navigationManager;

	public NotificationService(IHttpContextAccessor httpContextAccessor, NavigationManager navigationManager)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		_navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
	}

	public async ValueTask DisposeAsync()
	{
		if (!Disposed)
		{
			if (HubConnection is not null)
			{
				await HubConnection.DisposeAsync();
			}
			Disposed = true;
		}

		GC.SuppressFinalize(this);
	}

	public async Task StartAsync()
	{
		// Do nothing if the user is not authenticated or if the connection is already established.
		if (!(_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false) ||
			(HubConnection is not null && HubConnection.State != HubConnectionState.Disconnected))
		{
			return;
		}

		HubConnection = new HubConnectionBuilder()
								.WithUrl(_navigationManager.ToAbsoluteUri("/notifications"), options =>
								{
									options.UseDefaultCredentials = true;
									var cookieContainer = new CookieContainer(_httpContextAccessor.HttpContext.Request.Cookies.Count);
									foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies)
									{
										cookieContainer.Add(new Cookie(
																	cookie.Key,
																	WebUtility.UrlEncode(cookie.Value),
																	"/",
																	_navigationManager.ToAbsoluteUri("/").Host));
										options.Headers.Add(cookie.Key, cookie.Value);
									}

									options.Cookies = cookieContainer;
									options.HttpMessageHandlerFactory = _ =>
									{
										var clientHandler = new HttpClientHandler
										{
											PreAuthenticate = true,
											CookieContainer = cookieContainer,
											UseCookies = true,
											UseDefaultCredentials = true,
										};
										return clientHandler;
									};
								})
								.WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)])
								.Build();
		HubConnection.On<Notification>("ReceiveNotification", (notification) =>
		{
			OnNotificationReceived?.Invoke(notification);
		});

		try
		{
			await HubConnection.StartAsync();
		}
		catch (HttpRequestException)
		{
			// HttpRequestException indicates the server is not available or the user is not authenticated.
		}
	}

	public event Action<Notification> OnNotificationReceived = default!;

	private bool Disposed { get; set; }
	private HubConnection? HubConnection { get; set; }
}
