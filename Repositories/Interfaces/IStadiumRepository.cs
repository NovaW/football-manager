using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager {
    public interface IStadiumRepository
    {
        Task<Stadium> GetStadium(int stadiumId);
        Task<IEnumerable<Stadium>> GetAllStadiums();
        Task<Stadium> AddStadium(Stadium stadium);
        Task<Stadium> RemoveStadium(int stadiumId);
    }
}