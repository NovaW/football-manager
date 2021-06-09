namespace FootballManager.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int? HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
    }
}
