using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;

namespace FootballManagerApi
{
    public interface ITeamRepository
    {
        Task<Team> GetTeam(int teamId);
        Task<IEnumerable<Team>> GetAllTeams();
        Task<Team> AddTeam(Team team);
        Task<Team> RemoveTeam(int teamId);
        Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds);
        Task<Team> LinkTeamToStadium(int teamId, int stadiumId);
    }
}