using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ScoreSystem.Scoring
{
	[ApiController]
	[Route("[controller]")]
	public class ScoreController : ControllerBase
	{
		private readonly IScoreRepository _repository;

		public ScoreController(IScoreRepository repository)
		{
			_repository = repository;
		}

		[HttpPost("Store")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Store([FromBody, Required] UserScore score)
		{
			var response = await _repository.InsertAsync(score);

			if (!response.IsUserRegistered)
				return BadRequest($"User {score.Username} is not registered and can't report score");

			if (!response.IsSuccessStatusCode)
				return StatusCode(500, response.Message);

			return Ok(score);
		}
	}
}
