using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManager.Models;

namespace FootballManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetAllTeams()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<Team> GetTeam(int teamId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult AddTeam(Team team)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult RemoveTeam(int teamId)
        {
            throw new NotImplementedException();
        }
    }
}
