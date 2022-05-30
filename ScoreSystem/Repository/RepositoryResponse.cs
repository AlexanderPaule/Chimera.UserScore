using ScoreSystem.Users;

namespace ScoreSystem.Repository
{
	public class RepositoryResponse : IUserRepositoryResponse
	{
		public bool GenericError { get; set; }
		public bool IsDuplicated { get; set; }
		public bool IsSuccessStatusCode => !IsDuplicated && !GenericError;
		public string Message { get; set; }
	}
}