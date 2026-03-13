namespace FootballLeague.Models
{
    /// <summary>
    /// Модел за таблица players.
    /// </summary>
    public class Player
    {
        public int      PlayerId    { get; set; }
        public int      ClubId      { get; set; }
        public string   ClubName    { get; set; } = string.Empty;  // JOIN поле
        public string   FullName    { get; set; } = string.Empty;
        public DateTime BirthDate   { get; set; }
        public string   Position    { get; set; } = string.Empty;  // GK/DF/MF/FW
        public int?     ShirtNumber { get; set; }
        public string   Status      { get; set; } = "Active";      // Active/Injured/Suspended

        // Изчислено поле – не се пази в БД
        public int Age => DateTime.Today.Year - BirthDate.Year
                          - (DateTime.Today.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        public override string ToString() => FullName;
    }
}
