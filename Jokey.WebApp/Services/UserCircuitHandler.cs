using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Jokey.WebApp.Services;

internal sealed class UserCircuitHandler : CircuitHandler, IDisposable
{
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly UserService _userService;

	public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider, UserService userService)
	{
		_authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
		_userService = userService ?? throw new ArgumentNullException(nameof(userService));
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_authenticationStateProvider.AuthenticationStateChanged += AuthenticationChanged;

		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	private void AuthenticationChanged(Task<AuthenticationState> task)
	{
		_ = UpdateAuthentication(task);

		async Task UpdateAuthentication(Task<AuthenticationState> task)
		{
			try
			{
				var state = await task;
				_userService.SetUser(state.User);
			}
			catch
			{
			}
		}
	}

	public override async Task OnConnectionUpAsync(Circuit circuit,	CancellationToken cancellationToken)
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		_userService.SetUser(state.User);
	}

	public void Dispose()
	{
		_authenticationStateProvider.AuthenticationStateChanged -= AuthenticationChanged;
	}
}