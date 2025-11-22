namespace Jokey.Client.Services;

public interface INotificationService
{
	Task StartAsync();

	event Action<Notification> OnNotificationReceived;
}
