# DotNetCoreInjectorExtensions

DotNetCoreInjectorExtensions solves some restrictions that are currently exists in ASP.NET CORE. At the moment ASP.NET CORE doesn't allow you to inject properties into your objects. DotNetCoreInjectorExtensions  extends the native dependency injection in a manner it's done in Autofac via PropertiesAutowire. Moreover it gives you a possibility to resolve dependencies using DependencyResolver (this approach should be known for the ones who worked with ASP.NET MVC). Also it provides you an interesting conception of resolving configurations. This is an open source solution. More details you can find on github.


## How to enable properties injection?

1. Install a package in your Asp.Net Core Solution

	`Install-Package DotNetCoreInjectorExtensions -Version 1.0.3-beta`

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

## How to use a conception of configuration resolving?
1. Open appsettings.json and add your configuration, like:
		{
		  "Currencies": [
		    {
		      "Id": "1",
		      "Code": "USD",
		      "Symbol": "$"
		    },
		    {
		      "Id": "2",
		      "Code": "EUR",
		      "Symbol": "€"
		    }
		  ]
		}

2. Create a configuration class. By a convention, it must ends with "Configuration" keyword. Also it must extends List or Dictionary.
	
		public sealed class CurrenciesConfiguration : List<Currency>
		{
		}

		public sealed class Currency
		{
			public uint Id { get; set; }
			public string Code { get; set; }
			public string Symbol { get; set; }
		}
		
		// It's just one more sample that uses Dictionary
		public sealed class GamesConfiguration : Dictionary<uint, string>
		{
		}
		
3. Assign `ConfigurationAutowiredAttribute` to the configuration class

			[ConfigurationAutowired]
			public sealed class CurrenciesConfiguration : List<Currency>
			
4. Find your `Startup` class and your configuration, like:

			public IConfiguration Configuration { get; } // Don't forget to make this public property
			
			public Startup(IHostingEnvironment env)
			{
				var builder = new ConfigurationBuilder()
					.SetBasePath(env.ContentRootPath)
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

				Configuration = builder.Build();
			}
			
5. Find your `ConfigureServices` method and add the following line:

		services.AddCoinfigurationAutowired(Configuration);
		
### Extra components:

* `IgnorePropertyAutowiredAttribute` can be used to avoid injection for a certain property:

		public class TestClass
		{
			[IgnorePropertyAutowired]
			public object PropertyToInject { get; set; }
		}

* `DependencyResolver` can be used to resolve a particular dependency or get `IServiceProvider` itself (similar approach was on Asp.Net MVC). See samples below:

	* Get service object by type

	`var serviceObject = DependencyResolver.Current.GetService<IServiceObject>();`

	`var serviceObject = DependencyResolver.Current.GetService(typeof(IServiceObject));`

	* Get actual `IServiceProvider`

	`var currentServiceProvider = DependencyResolver.Current.GetServiceProvider();`

* `DependencyInjector` can be used to inject properties to a particular object using `IServiceProvider`:

	`DependencyInjector.Current.InjectProperties(_serviceProvider, objectWithProperties);`
	
* `Singleton` can be applied to your class to support singleton (thread safe):

		public class YourClass : Singleton<YourClass>
		{
		}
		
	Sample of usage:
	
	`var yourClass = YourClass.Current`
	
### Exceptions:		
`ServiceProviderNotFoundException` is thrown when `ServiceProvider` is not configured yet. In order to fix it, you have to setup `ServiceProvider` through `SetupServiceProvider` method.

`DependencyResolver.Current.SetupServiceProvider(serviceProvider);`

or

`DependencyInjector.Current.SetupServiceProvider(serviceProvider);`
