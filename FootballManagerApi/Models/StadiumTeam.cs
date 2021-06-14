using System;
using System.Collections.Generic;

namespace FootballManagerApi.Models
{
    public partial class StadiumTeam
    {
        public long Id { get; set; }
        public long StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        public long TeamId { get; set; }
        public Team Team { get; set; }
    }
}
