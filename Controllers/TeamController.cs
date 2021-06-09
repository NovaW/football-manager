using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManager.Models;

namespace FootballManager.Controllers
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
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddTeam(Team team)
        {
            await _teamRepository.AddTeam(team);
            return Ok();
        }

        [HttpDelete]
        [Route("{teamId}")]
        public async Task<ActionResult> RemoveTeam(int teamId)
        {
            await _teamRepository.RemoveTeam(teamId);
            return Ok();
        }
    }
}
