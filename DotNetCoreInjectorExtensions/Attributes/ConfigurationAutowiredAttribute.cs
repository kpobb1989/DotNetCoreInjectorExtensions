using System;

namespace DotNetCoreInjectorExtensions.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ConfigurationAutowiredAttribute : Attribute
	{
	}
}