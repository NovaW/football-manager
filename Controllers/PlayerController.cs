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
    public class PlayerController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<Player> GetPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult AddPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult RemovePlayer(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}
