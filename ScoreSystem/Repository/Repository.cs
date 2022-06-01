using Nest;
using ScoreSystem.Scoring;
using ScoreSystem.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreUser = ScoreSystem.Users.User;
using CoreUserScore = ScoreSystem.Scoring.UserScore;

namespace ScoreSystem.Repository
{
	internal class Repository : IUserRepository, IScoreRepository
	{
		private const int MaxElasticAllowedSize = 10000;

		private readonly ElasticClient _client;
		private readonly RepositoryTranslator _translator;

		public Repository(ElasticClient client, RepositoryTranslator translator)
		{
			_client = client;
			_translator = translator;
		}

		public async Task<IUserRepositoryResponse<CoreUser>> InsertAsync(CoreUser user)
		{
			var existingUser = await GetUser(user.Username);

			if (!existingUser.IsValid)
			{
				return new RepositoryResponse<CoreUser>
				{
					GenericError = true,
					Message = $"Something went wrong during the check operation [{existingUser.ServerError}]"
				};
			}

			if (existingUser.Documents.Any())
			{
				return new RepositoryResponse<CoreUser>
				{
					IsDuplicated = true,
					Message = $"User with Username [{user.Username}] already exist"
				};
			}

			var response = await _client
				.IndexDocumentAsync(_translator.Convert(user));

			if (!response.IsValid)
			{
				return new RepositoryResponse<CoreUser>
				{
					GenericError = true,
					Message = $"Something went wrong during the save operation [{response.ServerError}]"
				};
			}

			return new RepositoryResponse<CoreUser>
			{
				Message = $"Operation Successfully Complete",
				Object = user
			};
		}

		public async Task<IScoreRepositoryResponse<RegisteredUserScore>> InsertAsync(CoreUserScore score)
		{
			var existingUser = await GetUser(score.Username);

			if (!existingUser.IsValid)
			{
				return new RepositoryResponse<RegisteredUserScore>
				{
					GenericError = true,
					Message = $"Something went wrong during the check operation [{existingUser.ServerError}]"
				};
			}

			if (!existingUser.Documents.Any())
			{
				return new RepositoryResponse<RegisteredUserScore>
				{
					IsUserRegistered = false,
					Message = $"User with Username [{score.Username}] is not registered"
				};
			}

			var repositoryScore = _translator
				.Convert(score);

			var response = await _client
				.IndexDocumentAsync(repositoryScore);

			if (!response.IsValid)
			{
				return new RepositoryResponse<RegisteredUserScore>
				{
					GenericError = true,
					Message = $"Something went wrong during the save operation [{response.ServerError}]"
				};
			}

			return new RepositoryResponse<RegisteredUserScore>
			{
				Message = $"Operation Successfully Complete",
				Object = _translator.Convert(repositoryScore)
			};
		}

		public async Task<IScoreRepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>> GetHighestAsync(int howMuch)
		{
			var topScoresExtraction = await _client
				.SearchAsync<UserScore>(x => x
					.Aggregations(a => a
						.Terms("scores", u => u
							.Size(howMuch)
							.Field(f => f.Username)
							.Order(o => o.Descending("max_value"))
							.Aggregations(aa => aa.Max("max_value", tv => tv.Field(f => f.Value)))
						)
					)
					.Sort(so => so.Descending(x => x.Value)));

			if (!topScoresExtraction.IsValid)
			{
				return new RepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>
				{
					GenericError = true,
					Message = $"Something went wrong during the retriving operation [{topScoresExtraction.ServerError}]"
				};
			}

			var topScores = topScoresExtraction
				.Aggregations
				.Terms("scores")
				.Buckets
				.Select(x => new
				{
					Username = x.Key,
					Value = x.Max("max_value").Value
				})
				.ToList();

			var allScoresForTopPlacedUsers = await _client
				.SearchAsync<UserScore>(x => x
					.Size(MaxElasticAllowedSize)
					.Query(q => q.
						Bool(b => b.Should(s => s.FieldContains("username", topScores.Select(x => x.Username).ToArray())))
					));

			if (!allScoresForTopPlacedUsers.IsValid)
			{
				return new RepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>
				{
					GenericError = true,
					Message = $"Something went wrong during the retriving operation [{allScoresForTopPlacedUsers.ServerError}]"
				};
			}

			var topCoreScores = allScoresForTopPlacedUsers
				.Documents
				.Join(topScores, s => $"{s.Username}-{s.Value}", a => $"{a.Username}-{a.Value}", (s, a) => s)
				.Select(_translator.Convert)
				.OrderByDescending(x => x.Value)
				.ToList();

			return new RepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>
			{
				Message = $"Operation Successfully Complete",
				Object = topCoreScores
			};
		}


		private async Task<ISearchResponse<User>> GetUser(string username)
		{
			return await _client
				.SearchAsync<User>(x => x
					.Size(1)
					.Query(q => q.Match(m => m
						.Field(f => f.Username)
						.Query(username))
					));
		}
	}
}