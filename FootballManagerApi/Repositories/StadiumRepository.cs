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
        //public StadiumRepository(FootballManagerContext dbContext){
        //    _dbContext = dbContext;
        //}

        public StadiumRepository()
        {
            _dbContext = new FootballManagerContext();
        }

        public async Task<Stadium> GetStadium(int stadiumId){
            var stadium = await _dbContext.Stadium
                    .Include(x => x.StadiumTeam)
                        .ThenInclude(x => x.Team)
                    .FirstAsync(x => x.Id == stadiumId);
            return SanitizeStadium(stadium);
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
            throw new NotImplementedException();
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