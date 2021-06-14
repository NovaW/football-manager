using System;
using System.Collections.Generic;

namespace FootballManagerApi.Models
{
    public partial class Player
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? HeightInCentimeters { get; set; }
        public string DateOfBirth { get; set; } //TODO NW: change this to a DateTime
        public string Nationality { get; set; }
        public Team Team { get; set; }
        public long? TeamId { get; set; }
    }
}
