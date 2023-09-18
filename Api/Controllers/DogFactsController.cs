using System.Threading.Tasks;
using Api.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogFactsController : ControllerBase
    {
        private readonly DogFactsClient _dogFactsClient;

        public DogFactsController(DogFactsClient dogFactsClient)
        {
            _dogFactsClient = dogFactsClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetDogFactsAsync([FromQuery] int count = 10)
        {
            var dogFacts = await _dogFactsClient.GetFactsAsync(count);
            return Ok(dogFacts);
        }
    }
}
