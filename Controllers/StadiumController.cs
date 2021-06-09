using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManager.Models;

namespace FootballManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {

        private IStadiumRepository _stadiumRepository;

        public StadiumController(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stadium>>> GetAllStadiums()
        {
            var results = await _stadiumRepository.GetAllStadiums();
            return Ok(results);
        }

        [HttpGet]
        [Route("{stadiumId}")]
        public async Task<ActionResult<Stadium>> GetStadium(int stadiumId)
        {
            var result = await _stadiumRepository.GetStadium(stadiumId);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddStadium(Stadium stadium)
        {
            await _stadiumRepository.AddStadium(stadium);
            return Ok();
        }

        [HttpDelete]
        [Route("{stadiumId}")]
        public async Task<ActionResult> RemoveStadium(int stadiumId)
        {
            var result = await _stadiumRepository.RemoveStadium(stadiumId);
            if(result == null){
                return NotFound();
            }
            return Ok();
        }
    }
}
