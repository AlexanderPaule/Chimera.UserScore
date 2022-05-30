using System.Threading.Tasks;

namespace ScoreSystem.Scoring
{
	public interface IScoreRepository
	{
		Task<IScoreRepositoryResponse> InsertAsync(UserScore score);
	}
}