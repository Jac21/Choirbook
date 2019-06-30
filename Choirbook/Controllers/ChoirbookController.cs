using System;
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
            try
            {
                return await _choirbookService.Get();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetScore")]
        public async Task<ActionResult<Score>> Get(string id)
        {
            ActionResult<Score> score;

            try
            {
                score = await _choirbookService.Get(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return score ?? NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Score>> Create(Score score)
        {
            try
            {
                await _choirbookService.Create(score);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return CreatedAtRoute("GetScore", new {id = score.Id}, score);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Score scoreIn)
        {
            try
            {
                var score = await _choirbookService.Get(id);

                if (score == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrEmpty(scoreIn.Id))
                    scoreIn.Id = score.Id;

                await _choirbookService.Update(id, scoreIn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var score = await _choirbookService.Get(id);

                if (score == null)
                {
                    return NotFound();
                }

                await _choirbookService.Remove(score.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            return NoContent();
        }
    }
}