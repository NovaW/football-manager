using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
namespace FootballManager
{
    public class PlayerRepository : IPlayerRepository
    {
        private IPseudoDbContext _pseudoDbContext;
        public PlayerRepository(IPseudoDbContext pseudoDbContext){
            _pseudoDbContext = pseudoDbContext;
        }

        public async Task<Player> GetPlayer(int playerId){
            return await _pseudoDbContext.GetPlayer(playerId);
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            var results = await _pseudoDbContext.GetPlayers();
            return results.AsEnumerable();
        }

        public async Task<Player> AddPlayer(Player player){
            return await _pseudoDbContext.AddPlayer(player);
        }

        public async Task<Player> RemovePlayer(int playerId){
            return await _pseudoDbContext.RemovePlayer(playerId);
        }
    
        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            return await _pseudoDbContext.TransferPlayer(playerId, newTeamId);
            // var player = await _pseudoDbContext.GetPlayer(playerId);
            // if(player == null) {
            //     throw new ArgumentException($"No player with Id {playerId} exists.");
            // }

            // var team = await _pseudoDbContext.GetTeam(newTeamId);
            // if(team == null) {
            //     throw new ArgumentException($"No team with Id {newTeamId} exists.");
            // }

            // await _pseudoDbContext.RemovePlayer(playerId);
            // player.TeamId = newTeamId;
            // return await _pseudoDbContext.AddPlayer(player);
        }
    }
}