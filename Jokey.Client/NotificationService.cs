namespace Jokey.Client;

public sealed class NotificationService
{
	private IToastService _toastService;

	public NotificationService(IToastService toastService)
	{
		_toastService = toastService;
	}

	public void ShowNotification(string message)
	{
		_toastService.ShowInfo(message);
	}
}
