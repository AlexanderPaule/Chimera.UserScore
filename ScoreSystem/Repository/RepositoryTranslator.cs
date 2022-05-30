using CoreUser = ScoreSystem.Users.User;

namespace ScoreSystem.Repository
{
	internal class RepositoryTranslator
	{
		public User Convert(CoreUser user)
		{
			return new User
			{
				Username = user.Username
			};
		}
	}
}