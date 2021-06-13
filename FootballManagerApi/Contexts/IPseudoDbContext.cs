using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManagerApi {
    public interface IPseudoDbContext {
        Task<Player> GetPlayer(int playerId);
        Task<Stadium> GetStadium(int stadiumId);
        Task<Team> GetTeam(int teamId);

        Task<IQueryable<Player>> GetPlayers();
        Task<IQueryable<Stadium>> GetStadiums();
        Task<IQueryable<Team>> GetTeams();

        Task<Player> AddPlayer(Player player);
        Task<Stadium> AddStadium(Stadium stadium);
        Task<Team> AddTeam(Team team);

        Task<Player> RemovePlayer(int playerId);
        Task<Stadium> RemoveStadium(int stadiumId);
        Task<Team> RemoveTeam(int teamId);

        Task<(Team Team, Stadium Stadium)> LinkTeamAndStadium(int teamId, int stadiumId);
        Task<Player> TransferPlayer(int playerId, int newTeamId);
        Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds);
    }
}