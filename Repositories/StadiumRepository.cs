using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager
{
    public class StadiumRepository : IStadiumRepository
    {
        private IPseudoDbContext _pseudoDbContext;
        public StadiumRepository(IPseudoDbContext pseudoDbContext){
            _pseudoDbContext = pseudoDbContext;
        }

        public async Task<Stadium> GetStadium(int stadiumId){
            var stadiums = await _pseudoDbContext.GetStadiums();
            var result = stadiums.First(x => x.Id == stadiumId);
            return result; 
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiums(){
            var results = await _pseudoDbContext.GetStadiums();
            return results.AsEnumerable();
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            var result = await _pseudoDbContext.AddStadium(stadium);
            return result;
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            var stadium = await _pseudoDbContext.RemoveStadium(stadiumId);
            return stadium;
        }

        public async Task<Stadium> LinkStadiumToTeam(int stadiumId, int teamId){
            throw new System.NotImplementedException();
        }
    }
}