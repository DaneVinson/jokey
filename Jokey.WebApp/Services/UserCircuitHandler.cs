using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Jokey.WebApp.Services;

internal sealed class UserCircuitHandler : CircuitHandler, IDisposable
{
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly IUserContext _userContext;

	public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider, IUserContext userContext)
	{
		_authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
		_userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		//_authenticationStateProvider.AuthenticationStateChanged += AuthenticationChanged;

		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	//private void AuthenticationChanged(Task<AuthenticationState> task)
	//{
	//	_ = UpdateAuthentication(task);

	//	async Task UpdateAuthentication(Task<AuthenticationState> task)
	//	{
	//		try
	//		{
	//			var state = await task;
	//			_userContext.SetUserName(state.User.Identity?.Name ?? string.Empty);
	//		}
	//		catch
	//		{
	//		}
	//	}
	//}

	public override async Task OnConnectionUpAsync(Circuit circuit,	CancellationToken cancellationToken)
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		_userContext.SetUserName(state.User.Identity?.Name ?? string.Empty);
	}

	public void Dispose()
	{
		//_authenticationStateProvider.AuthenticationStateChanged -= AuthenticationChanged;
	}
}