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
		private readonly IUserRepository _repository;

		public UserController(IUserRepository repository)
		{
			_repository = repository;
		}

		[HttpPost("Register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PostUserAsync([FromBody, Required] User user)
		{
			var response = await _repository.InsertAsync(user);

			if (response.IsDuplicated)
				return BadRequest($"User {user.Username} already Exists");
			
			if (!response.IsSuccessStatusCode)
				return StatusCode(500, response.Message);

			return Ok(response.Object);
		}
	}
}
