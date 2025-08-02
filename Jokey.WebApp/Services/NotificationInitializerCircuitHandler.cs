using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Jokey.WebApp.Services;

public class NotificationInitializerCircuitHandler : CircuitHandler
{
    private readonly INotificationService _notificationService;

    public NotificationInitializerCircuitHandler(INotificationService notificationService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await _notificationService.StartAsync();
        await base.OnConnectionUpAsync(circuit, cancellationToken);
    }
    //public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    //{
    //    await _notificationService.StartAsync();
    //    await base.OnCircuitOpenedAsync(circuit, cancellationToken);
    //}
}
