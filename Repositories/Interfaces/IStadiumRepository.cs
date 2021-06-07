using System.Collections.Generic;
using FootballManager.Models;
using System.Threading.Tasks;

namespace FootballManager {
    public interface IStadiumRepository
    {
        Task<Stadium> GetStadium(int stadiumId);
        Task<IEnumerable<Stadium>> GetAllStadiums();
        Task AddStadium(Stadium stadium);
        Task RemoveStadium(int stadiumId);
    }
}