using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ScoreSystem.Users
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpPost("Register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RegisterAsync([FromBody, Required] User user)
		{
			var response = await _userRepository.InsertAsync(user);

			if (response.IsDuplicated)
				return BadRequest($"User {user.Username} already Exists");
			
			if (!response.IsSuccessStatusCode)
				return StatusCode(500, response.Message);

			return Ok(user);
		}
	}
}
