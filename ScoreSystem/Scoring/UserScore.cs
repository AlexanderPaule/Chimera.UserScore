using System;
using System.ComponentModel.DataAnnotations;

namespace ScoreSystem.Scoring
{
	public class UserScore
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public int? Value { get; set; }
	}
}