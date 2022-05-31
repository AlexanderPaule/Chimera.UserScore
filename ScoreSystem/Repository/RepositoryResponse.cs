using ScoreSystem.Scoring;
using ScoreSystem.Users;

namespace ScoreSystem.Repository
{
	public class RepositoryResponse<T> : IUserRepositoryResponse<T>, IScoreRepositoryResponse<T>
	{
		public bool GenericError { get; set; }
		public bool IsDuplicated { get; set; }
		public bool IsUserRegistered { get; set; } = true;
		public string Message { get; set; }
		public T Object { get; set; }

		public bool IsSuccessStatusCode => !IsDuplicated
			&& !GenericError
			&& IsUserRegistered
			&& Object != null;
	}
}