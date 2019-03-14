using DotNetCoreInjectorExtensions.Components;
using Microsoft.AspNetCore.Builder;

namespace DotNetCoreInjectorExtensions.Extensions
{
	public static class UseExtensions
	{
		public static IApplicationBuilder UseHttpContextPropertiesAutowired(this IApplicationBuilder app)
		{
			return app.Use(async (context, next) =>
			{
				context.RequestServices = DependencyResolver.Current.GetServiceProvider();
				await next();
			});
		}
	}
}
