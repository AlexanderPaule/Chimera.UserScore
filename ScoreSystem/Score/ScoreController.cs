using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ScoreSystem.Score
{
	[ApiController]
	[Route("[controller]")]
	public class ScoreController : ControllerBase
	{
		[HttpPost("Store")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Store()
		{
			return Ok();
		}
	}
}
