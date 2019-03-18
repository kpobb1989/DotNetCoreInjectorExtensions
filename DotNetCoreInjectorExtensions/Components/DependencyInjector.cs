using System;
using System.Linq;
using System.Reflection;
using DotNetCoreInjectorExtensions.Attributes;
using DotNetCoreInjectorExtensions.Exceptions;

namespace DotNetCoreInjectorExtensions.Components
{
	public sealed class DependencyInjector : Singleton<DependencyInjector>
	{
		private IServiceProvider _serviceProvider;

		/// <summary>
		/// Setup current ServiceProvider
		/// </summary>
		public void SetupServiceProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Inject properties to object
		/// </summary>
		/// <param name="instance">Instance of object</param>
		public void InjectProperties(object instance)
		{
			if (_serviceProvider == null)
			{
				throw new ServiceProviderNotFoundException($"ServiceProvider is not configured yet. You can set it up using {nameof(SetupServiceProvider)} method");
			}

			if (instance == null)
			{
				return;
			}

			foreach (var property in instance
				.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(s => s.CanRead && s.CanWrite))
			{
				if (property.GetCustomAttributes<IgnorePropertyAutowiredAttribute>().Any())
				{
					continue;
				}

				var propertyType = property.PropertyType;

				var obj = _serviceProvider.GetService(propertyType);

				if (obj != null)
				{
					property.SetValue(instance, obj);
				}
			}
		} 
	}
}