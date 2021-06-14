using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using FootballManagerApi.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FootballManagerApi
{
    public class PlayerRepository : IPlayerRepository
    {
        private FootballManagerContext _dbContext;

        //public PlayerRepository(FootballManagerContext dbContext){
        //    _dbContext = dbContext;
        //}
        public PlayerRepository()
        {
            _dbContext = new FootballManagerContext();
        }

        public async Task<Player> GetPlayer(int playerId){
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            return await _dbContext.Player
                .Select(p => SanitizePlayer(p))
                .ToListAsync();
        }

        public async Task<Player> AddPlayer(Player player){
            var result = _dbContext.Player.Add(player);
            await _dbContext.SaveChangesAsync();
            return SanitizePlayer(result.Entity);
        }

        public async Task<Player> RemovePlayer(int playerId){
            throw new NotImplementedException();
        }
    
        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            throw new NotImplementedException();
        }

        private static Player SanitizePlayer(Player player)
        {
            if(player == null) { return null; }
            return new Player
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                HeightInCentimeters = player.HeightInCentimeters,
                DateOfBirth = player.DateOfBirth,
                Nationality = player.Nationality,
                TeamId = player.TeamId,
                Team = player.Team == null ? null : new Team
                {
                    Id = player.Team.Id,
                    Name = player.Team.Name
                }
            };
        }
    }
}