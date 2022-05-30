using System.ComponentModel.DataAnnotations;

namespace ScoreSystem.Users
{
	public class User
	{
		[Required]
		public string Username { get; set; }
	}
}