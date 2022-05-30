using System;
using CoreUser = ScoreSystem.Users.User;
using CoreUserScore = ScoreSystem.Scoring.UserScore;

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

		internal Score Convert(CoreUserScore score)
		{
			return new Score
			{
				Username = score.Username,
				Value = score.Value.Value,
				OccurredOn = DateTimeOffset.Now
			};
		}
	}
}