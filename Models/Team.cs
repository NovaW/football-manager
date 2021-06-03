using System.Collections.Generic;
using System.Linq;

namespace FootballManager.Models
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
        }
        public string Name { get; set; }
        public string Location { get; set; }
        public HashSet<Player> Players { get; set; }

        public Stadium HomeStadium { get; set; }
    }
}
