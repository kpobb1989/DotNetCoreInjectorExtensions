using System;

namespace DotNetCoreInjectorExtensions.Exteptions
{
	public sealed class ServiceProviderNotFoundException : Exception
	{
		public ServiceProviderNotFoundException(string message): base(message)
		{
		}
	}
}