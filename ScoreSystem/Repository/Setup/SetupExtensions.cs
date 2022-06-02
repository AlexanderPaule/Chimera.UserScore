using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using ScoreSystem.Scoring;
using ScoreSystem.Users;
using System;

namespace ScoreSystem.Repository.Setup
{
	internal static class SetupExtensions
	{
		public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
		{
			var url = configuration["elasticsearch:url"];
			var defaultIndex = configuration["elasticsearch:index"];

			var settings = new ConnectionSettings(new Uri(url))
				.BasicAuthentication(configuration["elasticsearch:user"], configuration["elasticsearch:password"])
				.DefaultIndex(defaultIndex);

			var client = new ElasticClient(settings);

			client
				.Indices
				.Create(defaultIndex, index => index
					.Map<User>(x => x
						.AutoMap()
						.Properties(p => p.Keyword(k => k.Name(n => n.Username)))
					)
				);

			client
				.Indices
				.Create(defaultIndex, index => index
					.Map<UserScore>(x => x
						.AutoMap()
						.Properties(p => p.Keyword(k => k.Name(n => n.Username)))
					)
				);

			var repository = new Repository(client, new RepositoryTranslator());

			services.AddSingleton(repository);
			services.AddScoped<IUserRepository>(x => x.GetService<Repository>());
			services.AddScoped<IScoreRepository>(x => x.GetService<Repository>());
		}
	}
}
