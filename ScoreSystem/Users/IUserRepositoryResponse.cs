namespace ScoreSystem.Users
{
	public interface IUserRepositoryResponse
	{
		bool IsDuplicated { get; }
		bool IsSuccessStatusCode { get; }
		string Message { get; }
	}
}