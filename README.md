# DotNetCoreInjectorExtensions

It extends native .Net Core Dependency Injection. It gives you possibility to inject properties into your objects (similar approach is implemented in Autofac through PropertiesAutowired methods).

## How to use it?

1. Install a package in your Asp.Net Core Solution
`Install-Package DotNetCoreInjectorExtensions -Version 1.0.1-beta`

2. Open `Startup.cs` (or whatever you use for the startup)
* Find `ConfigureServices` method
* Change returned type from `void` to `IServiceProvider`
* Add `return DependencyResolver.Current.GetServiceProvider();` at the end of the method
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
		
### Notes:		
`ServiceProviderNotFoundException` is thrown when `ServiceProvider` is not configured yet. In order to fix it, you have to setup `ServiceProvider` through `SetupServiceProvider` method.

`DependencyResolver.Current.SetupServiceProvider(serviceProvider);`

### Extra features:

* `IgnorePropertyAutowiredAttribute` can be used to avoid injection for a certain property:

		public class TestClass
		{
			[IgnorePropertyAutowired]
			public object PropertyToInject { get; set; }
		}

* `DependencyResolver` can be used to resolve a particular dependency or get `IServiceProvider` itself (similar approach was on Asp.Net MVC). See samples below:

	Get service object by type

	`var serviceObject = DependencyResolver.Current.GetService<IServiceObject>();`

	`var serviceObject = DependencyResolver.Current.GetService(typeof(IServiceObject));`

	Get actual `IServiceProvider`

`var currentServiceProvider = DependencyResolver.Current.GetServiceProvider();`

* `DependencyInjector` can be used to inject properties using `IServiceProvider` to a particular object:

`DependencyInjector.Current.InjectProperties(_serviceProvider, objectWithProperties);`
