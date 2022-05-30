using ScoreSystem.Scoring;
using ScoreSystem.Users;

namespace ScoreSystem.Repository
{
	public class RepositoryResponse : IUserRepositoryResponse, IScoreRepositoryResponse
	{
		public bool GenericError { get; set; }
		public bool IsDuplicated { get; set; }
		public bool IsUserRegistered { get; set; } = true;
		public bool IsSuccessStatusCode => !IsDuplicated && !GenericError && IsUserRegistered;
		public string Message { get; set; }
	}
}