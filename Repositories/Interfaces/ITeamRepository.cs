using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager {
    public interface ITeamRepository
    {
        Task<Team> GetTeam(int teamId);
        Task<IEnumerable<Team>> GetAllTeams();
        Task<Team> AddTeam(Team team);
        Task<Team> RemoveTeam(int teamId);
        Task<Team> AddPlayersToTeam(int teamId, IEnumerable<Player> players);
        Task<Team> AddPlayersToTeamUsingIds(int teamId, IEnumerable<int> playerIds);
        Task<Team> LinkTeamToStadium(int teamId, int stadiumId);
    }
}