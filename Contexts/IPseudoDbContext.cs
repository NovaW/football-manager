using System.Collections.Generic;
using FootballManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager {
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

        Task LinkTeamAndStadium(int teamId, int stadiumId);
    }
}