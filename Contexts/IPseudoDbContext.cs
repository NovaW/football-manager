using System.Collections.Generic;
using FootballManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager {
    public interface IPseudoDbContext {
        Task<IQueryable<Player>> GetPlayers();
        Task<IQueryable<Stadium>> GetStadiums();
        Task<IQueryable<Team>> GetTeams();

        Task<Player> AddPlayer(Player player);

        Task<Stadium> AddStadium(Stadium stadium);

        Task<Team> AddTeam(Team team);
        Task<Player> RemovePlayer(int playerId);
        Task<Stadium> RemoveStadium(int stadiumId);

        Task<Team> RemoveTeam(int teamId);
    }
}