using System;
using DotNetCoreInjectorExtensions.Exceptions;

namespace DotNetCoreInjectorExtensions.Components
{
	public sealed class DependencyResolver : Singleton<DependencyResolver>
	{
		private IServiceProvider _serviceProvider;

		/// <summary>
		/// Setup current ServiceProvider
		/// </summary>
		/// <param name="serviceProvider">ServiceProvider</param>
		public void SetupServiceProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Get current ServiceProvider
		/// </summary>
		/// <returns>IServiceProvider</returns>
		public IServiceProvider GetServiceProvider() => _serviceProvider;

		/// <summary>
		/// Get the service object of the specified type.
		/// </summary>
		/// <param name="type">Type for resolving</param>
		/// <returns>Resolved object</returns>
		public object GetService(Type type)
		{
			if (_serviceProvider == null)
			{
				throw new ServiceProviderNotFoundException($"ServiceProvider is not configured yet. You can set it up using {nameof(SetupServiceProvider)} method");
			}

			return _serviceProvider.GetService(type);
		}

		/// <summary>
		/// Get the service object of the specified type.
		/// </summary>
		/// <typeparam name="T">Type for resolving</typeparam>
		/// <returns>Resolved object</returns>
		public T GetService<T>() where T : class
		{
			return GetService(typeof(T)) as T;
		}
	}
}
