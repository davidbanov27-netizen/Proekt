namespace FootballLeague.Models
{
    /// <summary>
    /// Модел за таблица clubs.
    /// </summary>
    public class Club
    {
        public int    ClubId    { get; set; }
        public string Name      { get; set; } = string.Empty;
        public string City      { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }

        public override string ToString() => Name;
    }
}
