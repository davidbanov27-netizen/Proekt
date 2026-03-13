namespace FootballLeague.Models
{
    public class League
    {
        public int    LeagueId { get; set; }
        public string Name     { get; set; } = string.Empty;
        public string Season   { get; set; } = string.Empty;

        public override string ToString() => $"{Name} ({Season})";
    }
}
