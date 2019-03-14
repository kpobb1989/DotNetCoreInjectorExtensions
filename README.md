# DotNetCoreInjectorExtensions

It extends native .Net Core Dependency Injection. It gives you possibility to inject properties into your objects (similar approach is implemented in Autofac through PropertiesAutowired methods).

## How to use it?

1. Install a package in your Asp.Net Core Solution
`Install-Package DotNetCoreInjectorExtensions -Version 1.0.1-beta`

2. Open `Startup.cs` (or whatever you use for the startup)
* Find `ConfigureServices` method
* Change returned type from `void` to `IServiceProvider`
* Add `return DependencyResolver.Current.GetServiceProvider()` at the end of the method;
* Add `services.AddPropertiesAutowired();`

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			...
			services.AddPropertiesAutowired();
			...
			return DependencyResolver.Current.GetServiceProvider();
		}

3.  Find `Configure` method and add `app.UseHttpContextPropertiesAutowired();`

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
		  ...
		  app.UseHttpContextPropertiesAutowired();
		  ...
		}

Extra features:

* `IgnorePropertyAutowiredAttribute` - you can use it if you don't wanna inject a certain property:

	public class TestClass
	{
		[IgnorePropertyAutowired]
		public object PropertyToInject { get; set; }
	}
	
* `DependencyResolver` - you can use it to reach current `IServiceProvider`. (similar approach was on Asp.Net MVC) and you even don't need to inject it, see a sample below:

`DependencyResolver.Current.GetService<YourObjectType>()` - returns instance of YourObjectType
`DependencyResolver.Current.GetServiceProvider();` - returns current `IServiceProvider`

