using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;

namespace FootballManagerApi
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(int playerId);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<Player> AddPlayer(Player player);
        Task<Player> RemovePlayer(int playerId);
        Task<Player> TransferPlayer(int playerId, int newTeamId);
    }
}