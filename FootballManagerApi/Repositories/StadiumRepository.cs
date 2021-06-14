using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using FootballManagerApi.Contexts;
using System;
using Microsoft.EntityFrameworkCore;
namespace FootballManagerApi
{
    public class StadiumRepository : IStadiumRepository
    {
        private FootballManagerContext _dbContext;

        public StadiumRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Stadium> GetStadium(int stadiumId){
            var matches = _dbContext.Stadium
                    .Include(x => x.StadiumTeam)
                        .ThenInclude(x => x.Team)
                    .Where(x => x.Id == stadiumId);

            if(!matches.Any()) { return null; }

            return SanitizeStadium(matches.First());
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiums(){
            return await _dbContext.Stadium
                .Include(x => x.StadiumTeam)
                    .ThenInclude(x => x.Team)
                .Select(x => SanitizeStadium(x))
                .ToListAsync();
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            var result = _dbContext.Stadium.Add(stadium);
            await _dbContext.SaveChangesAsync();
            return await GetStadium((int)result.Entity.Id);
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            var matches = await _dbContext.Stadium
                .Include(x => x.StadiumTeam)
                    .ThenInclude(x => x.Team)
                .Where(x => x.Id == stadiumId)
                .ToListAsync();

            if (!matches.Any()) { return null; }

            _dbContext.Stadium.Remove(matches.First());
            await _dbContext.SaveChangesAsync();
            return matches.First();
        }

        public async Task<Stadium> LinkStadiumToTeam(int stadiumId, int teamId){
            throw new NotImplementedException();
        }

        private static Stadium SanitizeStadium(Stadium stadium)
        {
            if(stadium == null) { return stadium; }
            return new Stadium
            {
                Id = stadium.Id,
                Name = stadium.Name,
                Location = stadium.Location,
                StadiumTeam = stadium.StadiumTeam == null ? null : new StadiumTeam
                {
                    Id = stadium.StadiumTeam.Id,
                    StadiumId = stadium.StadiumTeam.StadiumId,
                    TeamId = stadium.StadiumTeam.TeamId,
                    Team = stadium.StadiumTeam.Team == null ? null : new Team
                    {
                        Id = stadium.StadiumTeam.Team.Id,
                        Name = stadium.StadiumTeam.Team.Name
                    }
                }
            };
        }
    }
}