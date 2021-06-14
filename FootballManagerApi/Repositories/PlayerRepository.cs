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

        public PlayerRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Player> GetPlayer(int playerId){
            var matches = _dbContext.Player
                    .Include(x => x.Team)
                    .Where(x => x.Id == playerId);

            if(!matches.Any()) { return null; }

            return SanitizePlayer(matches.First());
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(){
            return await _dbContext.Player
                .Include(x => x.Team)
                .Select(p => SanitizePlayer(p))
                .ToListAsync();
        }

        public async Task<Player> AddPlayer(Player player){
            var result = _dbContext.Player.Add(player);
            await _dbContext.SaveChangesAsync();
            return await GetPlayer((int)result.Entity.Id);
        }

        public async Task<Player> RemovePlayer(int playerId){
            var matches = await _dbContext.Player
                .Include(x => x.Team)
                .Where(x => x.Id == playerId)
                .ToListAsync();

            if(!matches.Any()) { return null; }

            _dbContext.Player.Remove(matches.First());
            await _dbContext.SaveChangesAsync();
            return matches.First();
        }
    
        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            var player = await _dbContext.Player.FindAsync((long)playerId);
            player.TeamId = newTeamId;
            await _dbContext.SaveChangesAsync();
            return await GetPlayer(playerId);
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