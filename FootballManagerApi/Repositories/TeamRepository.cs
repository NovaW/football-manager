using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using FootballManagerApi.Contexts;
using Microsoft.EntityFrameworkCore;
namespace FootballManagerApi
{
    public class TeamRepository : ITeamRepository
    {
        private FootballManagerContext _dbContext;
        //public TeamRepository(FootballManagerContext dbContext){
        //    _dbContext = dbContext;
        //}
        public TeamRepository()
        {
            _dbContext = new FootballManagerContext();
        }

        public async Task<Team> GetTeam(int teamId) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetAllTeams() {
            return await _dbContext.Team
                .Include(x => x.StadiumTeam) // TODO NW: Is this necessary?
                .Select(x => SanitizeTeam(x))
                .ToListAsync();
        }

        public async Task<Team> AddTeam(Team team) {
            var result = _dbContext.Team.Add(team);
            await _dbContext.SaveChangesAsync();
            return SanitizeTeam(result.Entity);
        }

        public async Task<Team> RemoveTeam(int teamId) {
            throw new NotImplementedException();
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> LinkTeamToStadium(int teamId, int stadiumId) {
            var existingLinks = _dbContext.StadiumTeam
                .Where(x => x.TeamId == teamId || x.StadiumId == stadiumId);
            _dbContext.RemoveRange(existingLinks);

            //TODO NW: bug when linking teams and stadiums when links already exist

            await _dbContext.SaveChangesAsync();

            _dbContext.StadiumTeam
                .Add(new StadiumTeam { TeamId = teamId, StadiumId = stadiumId });

            await _dbContext.SaveChangesAsync();

            var team = _dbContext.Team.Find((long)teamId);
            return SanitizeTeam(team);
        }

        private static Team SanitizeTeam(Team team)
        {
            if(team == null) { return null; }
            return new Team
            {
                Id = team.Id,
                Name = team.Name,

                Players = team.Players?.Select(p => new Player
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    HeightInCentimeters = p.HeightInCentimeters,
                    DateOfBirth = p.DateOfBirth,
                    Nationality = p.Nationality,
                    TeamId = p.TeamId
                }).ToList(),

                StadiumTeam = team.StadiumTeam == null ? null : new StadiumTeam
                {
                    Id = team.StadiumTeam.Id,
                    TeamId = team.StadiumTeam.TeamId,
                    StadiumId = team.StadiumTeam.StadiumId
                }
            };

        }
    }
}