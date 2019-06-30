using System.Collections.Generic;
using System.Threading.Tasks;
using Choirbook.Models;
using Choirbook.Services;
using Microsoft.AspNetCore.Mvc;

namespace Choirbook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoirbookController : ControllerBase
    {
        private readonly ChoirbookService _choirbookService;

        public ChoirbookController(ChoirbookService choirbookService)
        {
            _choirbookService = choirbookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Score>>> Get()
        {
            return await _choirbookService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetScore")]
        public async Task<ActionResult<Score>> Get(string id)
        {
            var score = await _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            return score;
        }

        [HttpPost]
        public async Task<ActionResult<Score>> Create(Score score)
        {
            await _choirbookService.Create(score);

            return CreatedAtRoute("GetScore", new {id = score.Id}, score);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Score scoreIn)
        {
            var score = await _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(scoreIn.Id))
                scoreIn.Id = score.Id;

            await _choirbookService.Update(id, scoreIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var score = await _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            await _choirbookService.Remove(score.Id);

            return NoContent();
        }
    }
}