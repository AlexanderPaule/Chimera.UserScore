using System.Threading.Tasks;

namespace ScoreSystem.Users
{
	public interface IUserRepository
	{
		Task<IUserRepositoryResponse<User>> InsertAsync(User user);
	}
}