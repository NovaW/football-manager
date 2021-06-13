using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using FootballManagerApi.Contexts;
using System;

namespace FootballManagerApi
{
    public class StadiumRepository : IStadiumRepository
    {
        private FootballManagerContext _dbContext;
        public StadiumRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Stadium> GetStadium(int stadiumId){
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiums(){
            throw new NotImplementedException();
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            throw new NotImplementedException();
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            throw new NotImplementedException();
        }

        public async Task<Stadium> LinkStadiumToTeam(int stadiumId, int teamId){
            throw new NotImplementedException();
        }
    }
}