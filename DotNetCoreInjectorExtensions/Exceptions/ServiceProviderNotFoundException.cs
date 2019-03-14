using System;

namespace DotNetCoreInjectorExtensions.Exceptions
{
	public sealed class ServiceProviderNotFoundException : Exception
	{
		public ServiceProviderNotFoundException(string message): base(message)
		{
		}
	}
}