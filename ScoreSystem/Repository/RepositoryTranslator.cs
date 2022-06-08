using System;
using CoreUser = ScoreSystem.Users.User;
using CoreUserScore = ScoreSystem.Scoring.UserScore;
using CoreRegisteredUserScore = ScoreSystem.Scoring.RegisteredUserScore;

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

		internal CoreRegisteredUserScore Convert(UserScore score)
		{
			return new CoreRegisteredUserScore
			{
				Username = score.Username,
				Value = score.Value,
				OccurredOn = score.OccurredOn
			};
		}
	}
}