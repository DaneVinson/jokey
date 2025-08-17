namespace Jokey.WebApp.Services;

internal sealed class NotificationsCircuitHandler : CircuitHandler
{
	private readonly INotificationService _notificationService;

	public NotificationsCircuitHandler(INotificationService notification)
	{
		_notificationService = notification ?? throw new ArgumentNullException(nameof(notification));
	}

	public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		await _notificationService.StartAsync();

		await base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}
}