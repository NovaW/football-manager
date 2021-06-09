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
            return await _pseudoDbContext.GetStadium(stadiumId);
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiums(){
            var results = await _pseudoDbContext.GetStadiums();
            return results.AsEnumerable();
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            return await _pseudoDbContext.AddStadium(stadium);
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            return await _pseudoDbContext.RemoveStadium(stadiumId);
        }

        public async Task<Stadium> LinkStadiumToTeam(int stadiumId, int teamId){
            await _pseudoDbContext.LinkTeamAndStadium(teamId, stadiumId);
            return await _pseudoDbContext.GetStadium(stadiumId);
        }
    }
}