using Nest;
using ScoreSystem.Users;
using System.Linq;
using System.Threading.Tasks;
using CoreUser = ScoreSystem.Users.User;

namespace ScoreSystem.Repository
{
	// TODO: Introduce Retry Strategy
	internal class Repository : IUserRepository
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
			var existingUser = await _client.SearchAsync<User>(x => x
				.From(0)
				.Size(1)
				.Query(q => q
					.Match(m => m.Field(f => f.Username).Query(user.Username))
				));

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

			var response = await _client.IndexDocumentAsync(_translator.Convert(user));

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
	}
}