using Nest;
using ScoreSystem.Scoring;
using ScoreSystem.Users;
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

		public async Task<IUserRepositoryResponse> InsertAsync(CoreUser user)
		{
			var existingUser = await GetUser(user.Username);

			if (!existingUser.IsValid)
			{
				return new RepositoryResponse
				{
					GenericError = true,
					Message = $"Something went wrong during the check operation [{existingUser.ServerError}]"
				};
			}

			if (existingUser.Documents.Any())
			{
				return new RepositoryResponse
				{
					IsDuplicated = true,
					Message = $"User with Username [{user.Username}] already exist"
				};
			}

			var response = await _client
				.IndexDocumentAsync(_translator.Convert(user));

			if (!response.IsValid)
			{
				return new RepositoryResponse
				{
					GenericError = true,
					Message = $"Something went wrong during the save operation [{response.ServerError}]"
				};
			}

			return new RepositoryResponse
			{
				Message = $"Operation Successfully Complete"
			};
		}

		public async Task<IScoreRepositoryResponse> InsertAsync(CoreUserScore score)
		{
			var existingUser = await GetUser(score.Username);

			if (!existingUser.IsValid)
			{
				return new RepositoryResponse
				{
					GenericError = true,
					Message = $"Something went wrong during the check operation [{existingUser.ServerError}]"
				};
			}

			if (!existingUser.Documents.Any())
			{
				return new RepositoryResponse
				{
					IsUserRegistered = false,
					Message = $"User with Username [{score.Username}] is not registered"
				};
			}

			var response = await _client
				.IndexDocumentAsync(_translator.Convert(score));

			if (!response.IsValid)
			{
				return new RepositoryResponse
				{
					GenericError = true,
					Message = $"Something went wrong during the save operation [{response.ServerError}]"
				};
			}

			return new RepositoryResponse
			{
				Message = $"Operation Successfully Complete"
			};
		}

		private async Task<ISearchResponse<User>> GetUser(string username)
		{
			return await _client
				.SearchAsync<User>(x => x
					.From(0)
					.Size(1)
					.Query(q => q
						.Match(m => m.Field(f => f.Username).Query(username))
					));
		}
	}
}