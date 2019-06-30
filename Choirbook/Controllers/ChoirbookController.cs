using System.Collections.Generic;
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
        public ActionResult<List<Score>> Get() => _choirbookService.Get();

        [HttpGet("{id}:length(24)", Name = "GetScore")]
        public ActionResult<Score> Get(string id)
        {
            var score = _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            return score;
        }

        [HttpPost]
        public ActionResult<Score> Create(Score score)
        {
            _choirbookService.Create(score);

            return CreatedAtRoute("GetScore", new {id = score.Id}, score);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Score scoreIn)
        {
            var score = _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            _choirbookService.Update(id, scoreIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var score = _choirbookService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            _choirbookService.Remove(score.Id);

            return NoContent();
        }
    }
}