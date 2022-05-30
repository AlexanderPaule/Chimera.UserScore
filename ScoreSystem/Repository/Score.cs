using System;

namespace ScoreSystem.Repository
{
	internal class Score
	{
		public string Username { get; set; }
		public int Value { get; set; }
		public DateTimeOffset OccurredOn { get; set; }
	}
}