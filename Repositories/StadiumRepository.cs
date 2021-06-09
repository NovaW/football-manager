using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager
{
    public class StadiumRepository : IStadiumRepository
    {
        private IQueryable<Stadium> Stadiums;
        public StadiumRepository(){
            // I considered making this a dictionary to
            // ensure ID uniqueness, but figured an IQueryable
            // was more realistic. Right now IDs can clash
            Stadiums = new Stadium[] {}.AsQueryable();
        }

        // This method is synchronous, but I imagine a real DB
        // would be used at some point in the future, so I'm doing
        // this in preparation. Same story with the other methods.
        public async Task<Stadium> GetStadium(int stadiumId){
            var result = Stadiums.First(x => x.Id == stadiumId);
            return result; 
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiums(){
            var results = Stadiums.Select(x => x).AsEnumerable();
            return results;
        }

        public async Task AddStadium(Stadium stadium){
            var maxId = Stadiums.Count() == 0 ? 1 : Stadiums.Max(x => x.Id);
            stadium.Id = maxId++; //normally entity framework assigns IDs, but this works
            Stadiums = Stadiums.Append(stadium);
        }

        public async Task RemoveStadium(int stadiumId){
            // Would normally use the DbContext.Remove method
            Stadiums = Stadiums.Where(x => x.Id != stadiumId);
        }
    }
}