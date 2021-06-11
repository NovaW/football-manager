using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FootballManager
{
    public class TeamRepository : ITeamRepository
    {
        private IPseudoDbContext _pseudoDbContext;
        public TeamRepository(IPseudoDbContext pseudoDbContext){
            _pseudoDbContext = pseudoDbContext;
        }

        public async Task<Team> GetTeam(int teamId){
            return await _pseudoDbContext.GetTeam(teamId);
        }

        public async Task<IEnumerable<Team>> GetAllTeams(){
            var results = await _pseudoDbContext.GetTeams();
            return results.AsEnumerable();
        }

        public async Task<Team> AddTeam(Team team){
            return await _pseudoDbContext.AddTeam(team);
        }

        public async Task<Team> RemoveTeam(int teamId){
            return await _pseudoDbContext.RemoveTeam(teamId);
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds){

            var players = new List<Player>();
            foreach(var id in playerIds){
                var player = await _pseudoDbContext.GetPlayer(id);
                players.Add(player);
                if(player == null){
                    throw new ArgumentException($"No player with Id {id} exists.");
                }
            }
            
            foreach(var player in players){
                await _pseudoDbContext.RemovePlayer(player.Id);
                player.TeamId = teamId;
                await _pseudoDbContext.AddPlayer(player);
            }
            return await _pseudoDbContext.GetTeam(teamId);
        }

        public async Task<Team> LinkTeamToStadium(int teamId, int stadiumId){
            await _pseudoDbContext.LinkTeamAndStadium(teamId, stadiumId);
            return await _pseudoDbContext.GetTeam(teamId);
        }
    }
}