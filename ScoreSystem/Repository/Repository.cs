using Nest;
using ScoreSystem.Scoring;
using ScoreSystem.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreUser = ScoreSystem.Users.User;
using CoreUserScore = ScoreSystem.Scoring.UserScore;

namespace ScoreSystem.Repository
{
	// TODO: Introduce Retry Strategy
	internal class Repository : IUserRepository, IScoreRepository
	{
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

		public async Task<IScoreRepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>> GetHighestAsync(DateTimeOffset rangeFrom, DateTimeOffset rangeTo, int from, int howMuch)
		{
			var scores = await _client
				.SearchAsync<UserScore>(x => x
					.From(from)
					.Size(howMuch)
					.Query(q => q.DateRange(m => m
						.Field(f => f.OccurredOn)
						.GreaterThanOrEquals(DateMath.Anchored(rangeFrom.DateTime).RoundTo(DateMathTimeUnit.Second))
						.LessThanOrEquals(DateMath.Anchored(rangeTo.DateTime).RoundTo(DateMathTimeUnit.Second)))
					));

			if (!scores.IsValid)
			{
				return new RepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>
				{
					GenericError = true,
					Message = $"Something went wrong during the retriving operation [{scores.ServerError}]"
				};
			}

			return new RepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>
			{
				Message = $"Operation Successfully Complete",
				Object = scores.Documents.Select(_translator.Convert).ToList()
			};
		}


		private async Task<ISearchResponse<User>> GetUser(string username)
		{
			return await _client
				.SearchAsync<User>(x => x
					.From(0)
					.Size(1)
					.Query(q => q.Match(m => m
						.Field(f => f.Username)
						.Query(username))
					));
		}
	}
}