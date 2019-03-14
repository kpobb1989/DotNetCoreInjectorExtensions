using DotNetCoreInjectorExtensions.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreInjectorExtensions.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddPropertiesAutowired(this IServiceCollection services)
		{
			var serviceProvider = services.BuildServiceProvider();

			var serviceProviderDecorated = new ServiceProviderDecorator(serviceProvider);

			DependencyResolver.Current.SetupServiceProvider(serviceProviderDecorated);
		}
	}
}