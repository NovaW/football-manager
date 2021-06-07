using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager {
    public interface ITeamRepository
    {
        Task<Team> GetTeam(int teamId);
        Task<IEnumerable<Team>> GetAllTeams();
        Task AddTeam(Team team);
        Task RemoveTeam(int teamId);
    }
}