using System;

namespace ScoreSystem.Repository
{
	internal class UserScore
	{
		public string Username { get; set; }
		public int Value { get; set; }
		public DateTimeOffset OccurredOn { get; set; }
	}
}