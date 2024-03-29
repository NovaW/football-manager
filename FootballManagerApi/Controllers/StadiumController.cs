using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManagerApi.Models;

namespace FootballManagerApi.Controllers
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
        public async Task<ActionResult<Stadium>> AddStadium(Stadium stadium)
        {
            var result = await _stadiumRepository.AddStadium(stadium);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{stadiumId}")]
        public async Task<ActionResult<Stadium>> RemoveStadium(int stadiumId)
        {
            var result = await _stadiumRepository.RemoveStadium(stadiumId);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("LinkStadiumToTeam&stadiumId={stadiumId}&teamId={teamId}")]
        public async Task<ActionResult<Stadium>> LinkStadiumToTeam(int stadiumId, int teamId){
            var result = await _stadiumRepository.LinkStadiumToTeam(stadiumId, teamId);
            return Ok(result);
        }
    }
}
