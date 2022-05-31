namespace ScoreSystem.Scoring
{
	public interface IScoreRepositoryResponse<T>
	{
		bool IsUserRegistered { get; }
		bool IsSuccessStatusCode { get; }
		string Message { get; }
		T Object { get; }
	}
}