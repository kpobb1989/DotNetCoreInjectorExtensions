using System;

namespace DotNetCoreInjectorExtensions.Attributes
{
	/// <summary>
	/// Ignore property injection
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class IgnorePropertyAutowiredAttribute : Attribute
	{
	}
}