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

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<Player> players){
            foreach(var player in players){
                var playerDbEntry = await _pseudoDbContext.GetPlayer(player.Id);
                if(playerDbEntry != null){
                    //player already exists in the DB so we check if the models match
                    if(player.DetailsMatch(playerDbEntry)){
                        //if they do, we'll remove the old model
                        await _pseudoDbContext.RemovePlayer(player.Id);
                    }
                    else{
                        throw new ArgumentException($"Details given for player with Id {player.Id} doesn't match with existing player in system.");
                    }
                 }
                player.TeamId = teamId;
                await _pseudoDbContext.AddPlayer(player);
            }
            return await _pseudoDbContext.GetTeam(teamId);
        }

        public async Task<Team> AddPlayersToTeamUsingIds(int teamId, IEnumerable<int> playerIds){
            foreach(var id in playerIds){
                var player = await _pseudoDbContext.GetPlayer(id);
                await _pseudoDbContext.RemovePlayer(id);
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