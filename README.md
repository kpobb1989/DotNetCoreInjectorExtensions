# DotNetCoreInjectorExtensions

It extends native .Net Core Dependency Injection. It gives you possibility to inject properties into your objects (similar approach is implemented in Autofac through PropertiesAutowired methods).

## How to use it?

1. Install a package in your Asp.Net Core Solution
`Install-Package DotNetCoreInjectorExtensions -Version 1.0.1-beta`

2. Open `Startup.cs` (or whatever you use for the startup)

2.1 Find `ConfigureServices` method

2.1.1 Change returned type from `void` to `IServiceProvider`

2.1.2 Add `return DependencyResolver.Current.GetServiceProvider()` at the end of the method;

2.1.2 Add `services.AddPropertiesAutowired();`

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			...
			services.AddPropertiesAutowired();
			...
			return DependencyResolver.Current.GetServiceProvider();
		}

2.2  Find `Configure` method

2.2.1 Add `app.UseHttpContextPropertiesAutowired();`


		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
		  ...
		  app.UseHttpContextPropertiesAutowired();
		  ...
		}
