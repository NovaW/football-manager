using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FootballManager.Models;

namespace FootballManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
        {
            var results = await _playerRepository.GetAllPlayers();
            return Ok(results);
        }

        [HttpGet]
        [Route("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayer(int playerId)
        {
            var result = await _playerRepository.GetPlayer(playerId);
            if(result == null){
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddPlayer(Player player)
        {
            var result = await _playerRepository.AddPlayer(player);
            return Ok();
        }

        [HttpDelete]
        [Route("{playerId}")]
        public async Task<ActionResult> RemovePlayer(int playerId)
        {
            var result = await _playerRepository.RemovePlayer(playerId);
            if(result == null){
                return NotFound();
            }
            return Ok();
        }
    }
}
