using System.Collections.Generic;
using System.Linq;

namespace FootballManagerApi.Models
{
    public class Team
    {
        public Team()
        {
            Players = new Player[] {};
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public int? HomeStadiumId { get; set; }

        public Stadium HomeStadium { get; set; }
    }
}
