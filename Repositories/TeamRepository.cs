using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager{
    public class TeamRepository{
        private IQueryable<Team> Teams;
        public TeamRepository(){
            // I considered making this a dictionary to
            // ensure ID uniqueness, but figured an IQueryable
            // was more realistic. Right now IDs can clash
            Teams = new Team[] {}.AsQueryable();
        }

        // This method is synchronous, but I imagine a real DB
        // would be used at some point in the future, so I'm doing
        // this in preparation. Same story with the other methods.
        public async Task<Team> GetTeam(int teamId){
            var result = Teams.First(x => x.Id == teamId);
            return result; 
        }

        public async Task<IEnumerable<Team>> GetTeams(){
            var results = Teams.Select(x => x).AsEnumerable();
            return results;
        }

        public async Task AddTeam(Team team){
            var maxId = Teams.Max(x => x.Id);
            team.Id = maxId++; //normally entity framework assigns IDs, but this works
            Teams = Teams.Append(team);
        }

        public async Task RemoveTeam(int teamId){
            // Would normally use the DbContext.Remove method
            Teams = Teams.Where(x => x.Id != teamId);
        }
    }
}