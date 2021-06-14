using System;
using System.Collections.Generic;

namespace FootballManagerApi.Models
{
    public partial class Stadium
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public StadiumTeam StadiumTeam { get; set; }
    }
}
