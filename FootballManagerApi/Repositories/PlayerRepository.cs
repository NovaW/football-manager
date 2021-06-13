using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using FootballManagerApi.Contexts;

namespace FootballManagerApi
{
    public class PlayerRepository : IPlayerRepository
    {
        private FootballManagerContext _dbContext;

        public PlayerRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Player> GetPlayer(int playerId){
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            throw new NotImplementedException();
        }

        public async Task<Player> AddPlayer(Player player){
            throw new NotImplementedException();
        }

        public async Task<Player> RemovePlayer(int playerId){
            throw new NotImplementedException();
        }
    
        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            throw new NotImplementedException();
        }
    }
}