using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManagerApi.Models;

namespace FootballManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
        {
            var results = await _teamRepository.GetAllTeams();
            return Ok(results);
        }

        [HttpGet]
        [Route("{teamId}")]
        public async Task<ActionResult<Team>> GetTeam(int teamId)
        {
            var result = await _teamRepository.GetTeam(teamId);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Team>> AddTeam(Team team)
        {
            var result = await _teamRepository.AddTeam(team);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{teamId}")]
        public async Task<ActionResult<Team>> RemoveTeam(int teamId)
        {
            var result = await _teamRepository.RemoveTeam(teamId);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("AddPlayersToTeam&teamId={teamId}")]
        public async Task<ActionResult<Team>> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds)
        {
            var result = await _teamRepository.AddPlayersToTeam(teamId, playerIds);
            return Ok(result);
        }

        [HttpPost]
        [Route("LinkTeamToStadium&teamId={teamId}&stadiumId={stadiumId}")]
        public async Task<ActionResult<Team>> LinkTeamToStadium(int teamId, int stadiumId)
        {
            var result = await _teamRepository.LinkTeamToStadium(teamId, stadiumId);
            return Ok(result);
        }

    }
}
