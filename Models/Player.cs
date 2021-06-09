using System;
namespace FootballManager.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int HeightInCentimeters { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public int? TeamId{ get; set; }
        public Team Team { get; set; }
    }

    public static class PlayerExtensions{
        public static bool DetailsMatch(this Player firstPlayer, Player secondPlayer){
            return firstPlayer.FirstName == secondPlayer.FirstName
                    && firstPlayer.LastName == secondPlayer.LastName
                    && firstPlayer.HeightInCentimeters == secondPlayer.HeightInCentimeters
                    && firstPlayer.DateOfBirth == secondPlayer.DateOfBirth
                    && firstPlayer.Nationality == secondPlayer.Nationality;
        }
    }
}
