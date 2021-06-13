using System;
using System.Collections.Generic;

namespace FootballManagerApi.Models
{
    public partial class StadiumTeamLink
    {
        public long StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        public long TeamId { get; set; }
        public Team Team { get; set; }
    }
}
