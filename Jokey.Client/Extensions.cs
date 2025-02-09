namespace Jokey.Client;

public static class Extensions
{
	public static T GetConfigurationObject<T>(this WebAssemblyHostConfiguration configuration, string? key = null)
	{
		return configuration.GetSection(GetKey<T>(key)).Get<T>() ??
			throw new InvalidOperationException($"{nameof(WebAssemblyHostConfiguration)} could not bind configuration key \"{GetKey<T>(key)}\" to type {typeof(T).FullName}");

		string GetKey<T1>(string? testKey)
		{
			return testKey ?? typeof(T).Name;
		}
	}
}
