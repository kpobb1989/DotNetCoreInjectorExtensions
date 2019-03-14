using System;
using System.Linq;
using System.Reflection;
using DotNetCoreInjectorExtensions.Attributes;

namespace DotNetCoreInjectorExtensions.Components
{
	public sealed class DependencyInjector : Singleton<DependencyInjector>
	{
		/// <summary>
		/// Inject properties to object
		/// </summary>
		/// <param name="serviceProvider">IServiceProvider</param>
		/// <param name="instance">Instance of object</param>
		public void InjectProperties(IServiceProvider serviceProvider, object instance)
		{
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

				var obj = serviceProvider.GetService(propertyType);

				if (obj != null)
				{
					property.SetValue(instance, obj);
				}
			}
		} 
	}
}