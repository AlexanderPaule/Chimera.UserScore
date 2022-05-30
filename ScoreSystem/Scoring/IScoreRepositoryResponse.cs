namespace ScoreSystem.Scoring
{
	public interface IScoreRepositoryResponse
	{
		bool IsUserRegistered { get; }
		bool IsSuccessStatusCode { get; }
		string Message { get; }
	}
}