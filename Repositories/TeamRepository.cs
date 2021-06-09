using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager
{
    public class TeamRepository : ITeamRepository
    {
        private IPseudoDbContext _pseudoDbContext;
        public TeamRepository(IPseudoDbContext pseudoDbContext){
            _pseudoDbContext = pseudoDbContext;
        }

        public async Task<Team> GetTeam(int teamId){
            var teams = await _pseudoDbContext.GetTeams();
            var result = teams.First(x => x.Id == teamId);
            return result; 
        }

        public async Task<IEnumerable<Team>> GetAllTeams(){
            var results = await _pseudoDbContext.GetTeams();
            return results.AsEnumerable();
        }

        public async Task<Team> AddTeam(Team team){
            var result = await _pseudoDbContext.AddTeam(team);
            return result;
        }

        public async Task<Team> RemoveTeam(int teamId){
            var team = await _pseudoDbContext.RemoveTeam(teamId);
            return team;
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<Player> players){
            throw new System.NotImplementedException();
        }

        public async Task<Team> AddPlayersToTeamUsingIds(int teamId, IEnumerable<int> playerIds){
            throw new System.NotImplementedException();
        }

        public async Task<Team> LinkTeamToStadium(int teamId, int stadiumId){
            throw new System.NotImplementedException();
        }
    }
}