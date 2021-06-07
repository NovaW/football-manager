using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(int playerId);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task AddPlayer(Player player);
        Task RemovePlayer(int playerId);
    }
}