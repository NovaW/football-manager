using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using FootballManagerApi.Contexts;

namespace FootballManagerApi
{
    public class TeamRepository : ITeamRepository
    {
        private FootballManagerContext _dbContext;
        public TeamRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Team> GetTeam(int teamId){
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetAllTeams(){
            throw new NotImplementedException();
        }

        public async Task<Team> AddTeam(Team team){
            throw new NotImplementedException();
        }

        public async Task<Team> RemoveTeam(int teamId){
            throw new NotImplementedException();
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> LinkTeamToStadium(int teamId, int stadiumId){
            throw new NotImplementedException();
        }
    }
}