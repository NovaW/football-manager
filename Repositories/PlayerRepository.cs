using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager
{
    public class PlayerRepository : IPlayerRepository
    {
        private IPseudoDbContext _pseudoDbContext;
        public PlayerRepository(IPseudoDbContext pseudoDbContext){
            _pseudoDbContext = pseudoDbContext;
        }

        public async Task<Player> GetPlayer(int playerId){
            var players = await _pseudoDbContext.GetPlayers();
            var result = players.First(x => x.Id == playerId);
            return result; 
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            var results = await _pseudoDbContext.GetPlayers();
            return results.AsEnumerable();
        }

        public async Task<Player> AddPlayer(Player player){
            var result = await _pseudoDbContext.AddPlayer(player);
            return result;
        }

        public async Task<Player> RemovePlayer(int playerId){
            var player = await _pseudoDbContext.RemovePlayer(playerId);
            return player;
        }
    
        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            throw new System.NotImplementedException();
        }
    }
}