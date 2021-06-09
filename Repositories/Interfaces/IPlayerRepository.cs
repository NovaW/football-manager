using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(int playerId);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<Player> AddPlayer(Player player);
        Task<Player> RemovePlayer(int playerId);
    }
}