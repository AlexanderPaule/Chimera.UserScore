using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreSystem.Scoring
{
	public interface IScoreRepository
	{
		Task<IScoreRepositoryResponse<RegisteredUserScore>> InsertAsync(UserScore score);
		Task<IScoreRepositoryResponse<IReadOnlyCollection<RegisteredUserScore>>> GetHighestAsync(int howMuch);
	}
}