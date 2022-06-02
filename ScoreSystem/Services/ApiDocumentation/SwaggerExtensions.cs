using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace PhotoSi.Sales.Services.ApiDocumentation
{
	internal static class SwaggerExtensions
	{
		private const string BackendDoc = "BackendApiDocumentation";

		public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
		{
			services.AddSwaggerGen(config =>
			{
				config.SwaggerDoc(BackendDoc, CreateApiInfo());

				config.DocumentFilter<ServersCleanupFilter>();
				config.SchemaFilter<EnumSchemaFilter>();

				config.ResolveConflictingActions(apiDescriptions => apiDescriptions.Last());
			});

			return services;
		}

		private static OpenApiInfo CreateApiInfo()
		{
			return new OpenApiInfo
			{
				Title = "Chimera Score System",
				Description = "Server API Documentation",
				Contact = new OpenApiContact
				{
					Name = "Chimera Score System",
					Url = new Uri("https://github.com/AlexanderPaule/Chimera.UserScore")
				}
			};
		}

		public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint($"/swagger/{BackendDoc}/swagger.json", BackendDoc);
			});

			return app;
		}
	}
}