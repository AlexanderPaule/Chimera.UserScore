using System;

namespace ScoreSystem.Scoring
{
	public class RegisteredUserScore : UserScore
	{
		public DateTimeOffset OccurredOn { get; set; }
	}
}