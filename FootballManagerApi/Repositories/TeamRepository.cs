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

        public TeamRepository(FootballManagerContext dbContext){
            _dbContext = dbContext;
        }

        public async Task<Team> GetTeam(int teamId) {
            var matches = _dbContext.Team
                .Include(x => x.StadiumTeam)
                    .ThenInclude(x => x.Stadium)
                .Include(x => x.Players)
                .Where(x => x.Id == teamId);

            if(!matches.Any()) { return null; }

            return SanitizeTeam(matches.First());
        }

        public async Task<IEnumerable<Team>> GetAllTeams() {
            return await _dbContext.Team
                .Include(x => x.StadiumTeam)
                    .ThenInclude(x => x.Stadium)
                .Include(x => x.Players)
                .Select(x => SanitizeTeam(x))
                .ToListAsync();
        }

        public async Task<Team> AddTeam(Team team) {
            var result = _dbContext.Team.Add(team);
            await _dbContext.SaveChangesAsync();
            return await GetTeam((int)result.Entity.Id);
        }

        public async Task<Team> RemoveTeam(int teamId) {
            var matches = await _dbContext.Team
                .Include(x => x.StadiumTeam)
                    .ThenInclude(x => x.Stadium)
                .Include(x => x.Players)
                .Where(x => x.Id == teamId)
                .ToListAsync();

            if (!matches.Any()) { return null; }

            _dbContext.Team.Remove(matches.First());
            await _dbContext.SaveChangesAsync();
            return matches.First();
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds)
        {
            var players = _dbContext.Player.Where(x => playerIds.Contains((int)x.Id));
            foreach (var player in players)
            {
                player.TeamId = teamId;
            }
            await _dbContext.SaveChangesAsync();
            return await GetTeam(teamId);
        }

        public async Task<Team> LinkTeamToStadium(int teamId, int stadiumId) {
            var existingLinks = _dbContext.StadiumTeam
                .Where(x => x.TeamId == teamId || x.StadiumId == stadiumId);
            _dbContext.RemoveRange(existingLinks);

            await _dbContext.SaveChangesAsync();

            _dbContext.StadiumTeam
                .Add(new StadiumTeam { TeamId = teamId, StadiumId = stadiumId });

            await _dbContext.SaveChangesAsync();

            return await GetTeam(teamId);
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
                    StadiumId = team.StadiumTeam.StadiumId,
                    Stadium = team.StadiumTeam.Stadium == null ? null : new Stadium
                    {
                        Id = team.StadiumTeam.Stadium.Id,
                        Name = team.StadiumTeam.Stadium.Name,
                        Location = team.StadiumTeam.Stadium.Location
                    }
                }
            };

        }
    }
}