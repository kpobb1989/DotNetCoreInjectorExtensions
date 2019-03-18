using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotNetCoreInjectorExtensions.Exceptions;
using Microsoft.Extensions.DependencyInjection;

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
		/// Get the service object of <paramref name="serviceType" /> 
		/// </summary>
		/// <param name="serviceType">Type for resolving</param>
		/// <returns>Resolved object</returns>
		public object GetService(Type serviceType)
		{
			if (_serviceProvider == null)
			{
				throw new ServiceProviderNotFoundException($"ServiceProvider is not configured yet. You can set it up using {nameof(SetupServiceProvider)} method");
			}

			return _serviceProvider.GetService(serviceType);
		}

		/// <summary>
		/// Returns a service object of <typeparamref name="T" /> or null if there is no service object of type serviceType.
		/// </summary>
		/// <typeparam name="T">Type for resolving</typeparam>
		/// <returns>Resolved object</returns>
		public T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		/// <summary>
		/// Get an enumeration of services of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" />.
		/// </summary>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>An enumeration of services of type <paramref name="serviceType" />.</returns>
		public IEnumerable<object> GetServices(Type serviceType)
		{
			Type serviceType1 = typeof(IEnumerable<>).MakeGenericType(serviceType);

			var result = (IEnumerable<object>)GetRequiredService(serviceType1);

			return result;
		}

		/// <summary>
		/// Get an enumeration of services of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
		/// </summary>
		/// <typeparam name="T">The type of service object to get.</typeparam>
		/// <returns>An enumeration of services of type <typeparamref name="T" />.</returns>
		public IEnumerable<T> GetServices<T>()
		{
			return (IEnumerable<T>)GetServices(typeof(T));
		}

		/// <summary>
		/// Returns a service object of <paramref name="serviceType" />. Throws an InvalidOperationException if there is no service of type serviceType.
		/// </summary>
		/// <param name="serviceType">Service type</param>
		/// <returns>Resolved object or exception</returns>
		public object GetRequiredService(Type serviceType)
		{
			if (serviceType == null)
				throw new ArgumentNullException(nameof(serviceType));

			object service = GetService(serviceType);

			if (service is IEnumerable enumerable && !enumerable.Cast<object>().Any())
			{
				throw new InvalidOperationException($"{serviceType.Name} service is not found.");
			}

			if (service != null)
				return service;

			throw new InvalidOperationException($"{serviceType.Name} service is not found.");
		}

		/// <summary>
		/// Returns a service object of <typeparamref name="T" />. Throws an InvalidOperationException if there is no service of type serviceType.
		/// </summary>
		/// <typeparam name="T">Service type</typeparam>
		/// <returns>Resolved object or exception</returns>
		public T GetRequiredService<T>() where T : class
		{
			object service = GetService<T>();

			if (service != null)
				return (T)service;

			throw new InvalidOperationException($"{typeof(T).Name} service is not found.");
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> that can be used to resolve scoped services.
		/// </summary>
		/// <returns>IServiceScope</returns>
		public IServiceScope CreateScope()
		{
			return GetRequiredService<IServiceScopeFactory>().CreateScope();
		}
	}
}