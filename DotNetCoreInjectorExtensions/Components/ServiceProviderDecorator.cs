using System;

namespace DotNetCoreInjectorExtensions.Components
{
	internal sealed class ServiceProviderDecorator : IServiceProvider
	{
		private readonly IServiceProvider _provider;

		public ServiceProviderDecorator(IServiceProvider provider)
		{
			_provider = provider;
		}

		public object GetService(Type serviceType)
		{
			var service = _provider.GetService(serviceType);

			DependencyInjector.Current.InjectProperties(service);

			return service;
		}
	}
}