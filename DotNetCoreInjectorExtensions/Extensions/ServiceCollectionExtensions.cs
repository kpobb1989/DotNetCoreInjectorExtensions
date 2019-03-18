using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetCoreInjectorExtensions.Attributes;
using DotNetCoreInjectorExtensions.Components;
using Microsoft.Extensions.Configuration;
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

			DependencyInjector.Current.SetupServiceProvider(serviceProviderDecorated);
		}

		public static void AddCoinfigurationAutowired(this IServiceCollection services, IConfiguration configuration)
		{
			var configurations = Assembly.GetEntryAssembly().DefinedTypes.Where(s => s.GetCustomAttributes<ConfigurationAutowiredAttribute>().Any());

			foreach (var config in configurations)
			{
				var name = config.Name.Replace("Configuration", "");

				var instance = Activator.CreateInstance(config.AsType());

				if (instance is IDictionary)
				{
					ConfigureDictionary(services, configuration, name, (dynamic)instance);
				}
				else
				{
					Configure(services, configuration, name, (dynamic)instance);
				}
			}
		}


		private static void Configure<T>(IServiceCollection services, IConfiguration configuration, string name, T value)
		{
			typeof(OptionsConfigurationServiceCollectionExtensions)
				.GetMethod("Configure", new[] { typeof(IServiceCollection), typeof(IConfiguration) })
				.MakeGenericMethod(value.GetType())
				.Invoke(null, new object[] { services, configuration.GetSection(name) });
		}

		private static void ConfigureDictionary<T>(IServiceCollection services, IConfiguration configuration, string name, IDictionary<uint, T> value)
		{
			void Act(dynamic s)
			{
				ToDictionary(name, configuration, s);
			}

			typeof(OptionsServiceCollectionExtensions)
				.GetMethod("Configure", new[] { typeof(IServiceCollection), typeof(Action<object>) })
				.MakeGenericMethod(value.GetType())
				.Invoke(null, new object[] { services, (Action<dynamic>) Act });
		}


		private static void ToDictionary<TValue>(string sectionName, IConfiguration configuration, IDictionary<uint, TValue> dest)
		{
			var dictionary = configuration
				.GetSection(sectionName)
				.Get<Dictionary<string, TValue>>()
				.ToDictionary(x => uint.Parse(x.Key), x => x.Value);

			foreach (var item in dictionary)
			{
				dest.Add(item.Key, item.Value);
			}
		}
	}
}