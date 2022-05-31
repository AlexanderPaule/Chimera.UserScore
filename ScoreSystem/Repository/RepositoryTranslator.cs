using System;
using CoreUser = ScoreSystem.Users.User;
using CoreUserScore = ScoreSystem.Scoring.UserScore;
using CoreegisteredUserScore = ScoreSystem.Scoring.RegisteredUserScore;

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

		internal UserScore Convert(CoreUserScore score)
		{
			return new UserScore
			{
				Username = score.Username,
				Value = score.Value.Value,
				OccurredOn = DateTimeOffset.UtcNow
			};
		}

		internal CoreegisteredUserScore Convert(UserScore score)
		{
			return new CoreegisteredUserScore
			{
				Username = score.Username,
				Value = score.Value,
				OccurredOn = score.OccurredOn
			};
		}
	}
}