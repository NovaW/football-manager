using System;
using System.Collections.Generic;

namespace FootballManagerApi.Models
{
    public partial class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public StadiumTeam StadiumTeam { get; set; }
    }
}
