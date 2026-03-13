namespace FootballLeague.Models
{
    /// <summary>
    /// Модел за таблица transfers.
    /// </summary>
    public class Transfer
    {
        public int      TransferId    { get; set; }
        public int      PlayerId      { get; set; }
        public string   PlayerName    { get; set; } = string.Empty;  // JOIN
        public int?     FromClubId    { get; set; }
        public string   FromClubName  { get; set; } = "Свободен агент";  // JOIN
        public int      ToClubId      { get; set; }
        public string   ToClubName    { get; set; } = string.Empty;  // JOIN
        public DateTime TransferDate  { get; set; }
        public decimal? Fee           { get; set; }
        public string   Note          { get; set; } = string.Empty;
    }
}
