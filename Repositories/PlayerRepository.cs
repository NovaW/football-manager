using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager
{
    public class PlayerRepository : IPlayerRepository
    {
        private IQueryable<Player> Players;
        public PlayerRepository(){
            // I considered making this a dictionary to
            // ensure ID uniqueness, but figured an IQueryable
            // was more realistic. Right now IDs can clash
            Players = new Player[] {}.AsQueryable();
        }

        // This method is synchronous, but I imagine a real DB
        // would be used at some point in the future, so I'm doing
        // this in preparation. Same story with the other methods.
        public async Task<Player> GetPlayer(int playerId){
            var result = Players.First(x => x.Id == playerId);
            return result; 
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            var results = Players.Select(x => x).AsEnumerable();
            return results;
        }

        public async Task<Player> AddPlayer(Player player){
            var maxId = Players.Count() == 0 ? 1 : Players.Max(x => x.Id);
            player.Id = maxId++; //normally entity framework assigns IDs, but this works
            Players = Players.Append(player);
            return player;
        }

        public async Task<Player> RemovePlayer(int playerId){
            // Would normally use the DbContext.Remove method
            var player = Players.First(x => x.Id == playerId);
            Players = Players.Where(x => x.Id != playerId);
            return player;
        }
    }
}