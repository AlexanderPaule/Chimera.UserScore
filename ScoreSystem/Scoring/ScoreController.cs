using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
		public async Task<IActionResult> PostScoreAsync([FromBody, Required] UserScore score)
		{
			var response = await _repository.InsertAsync(score);

			if (!response.IsUserRegistered)
				return BadRequest($"User {score.Username} is not registered and can't report score");

			if (!response.IsSuccessStatusCode)
				return StatusCode(500, response.Message);

			return Ok(response.Object);
		}

		[HttpGet("Store")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetScoreAsync(
			[FromQuery] DateTimeOffset rangeFrom, [FromQuery] DateTimeOffset rangeTo,
			[FromQuery] int from = 0, [FromQuery] int howMuch = 10)
		{
			var now = DateTimeOffset.Now;

			if (now < rangeFrom)
				return BadRequest($"[{nameof(rangeFrom)}] must contain only past time info");
			
			if (rangeTo == default)
				rangeTo = now;

			if (rangeFrom > rangeTo)
				return BadRequest($"[{nameof(rangeFrom)}] can't be grater than [{nameof(rangeTo)}]");

			var response = await _repository
				.GetHighestAsync(rangeFrom, rangeTo, from, howMuch);

			if (!response.IsSuccessStatusCode)
				return StatusCode(500, response.Message);

			return Ok(response.Object);
		}
	}
}
