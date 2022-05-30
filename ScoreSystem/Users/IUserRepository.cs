using System.Threading.Tasks;

namespace ScoreSystem.Users
{
	public interface IUserRepository
	{
		Task<IUserRepositoryResponse> InsertAsync(User user);
	}
}