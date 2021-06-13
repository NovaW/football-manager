using System.Collections.Generic;
using FootballManagerApi.Models;
using System.Threading.Tasks;

namespace FootballManagerApi
{
    public interface IStadiumRepository
    {
        Task<Stadium> GetStadium(int stadiumId);
        Task<IEnumerable<Stadium>> GetAllStadiums();
        Task<Stadium> AddStadium(Stadium stadium);
        Task<Stadium> RemoveStadium(int stadiumId);
        Task<Stadium> LinkStadiumToTeam(int stadiumId, int teamId);
    }
}