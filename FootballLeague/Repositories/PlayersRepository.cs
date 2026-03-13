using FootballLeague.Data;
using FootballLeague.Models;
using System.Data;

namespace FootballLeague.Repositories
{
    /// <summary>
    /// Репозитори за играчи — целият SQL е тук.
    /// </summary>
    internal class PlayersRepository
    {
        // ── LIST + FILTER ────────────────────────────────────────────────────────
        /// <summary>
        /// Връща играчи с опционални филтри по клуб, позиция и търсене по ime.
        /// Всички филтри са комбинируеми.
        /// </summary>
        public List<Player> GetPlayers(int? clubId = null,
                                       string? position = null,
                                       string? nameLike = null)
        {
            var sql = @"
                SELECT p.PlayerId, p.ClubId, c.Name AS ClubName,
                       p.FullName, p.BirthDate, p.Position,
                       p.ShirtNumber, p.Status
                FROM   players p
                JOIN   clubs   c ON c.ClubId = p.ClubId
                WHERE  1=1";

            var parameters = new List<MySql.Data.MySqlClient.MySqlParameter>();

            if (clubId.HasValue && clubId.Value > 0)
            {
                sql += " AND p.ClubId = @ClubId";
                parameters.Add(Data.Db.Param("@ClubId", clubId.Value));
            }
            if (!string.IsNullOrWhiteSpace(position) && position != "Всички")
            {
                sql += " AND p.Position = @Position";
                parameters.Add(Data.Db.Param("@Position", position));
            }
            if (!string.IsNullOrWhiteSpace(nameLike))
            {
                sql += " AND p.FullName LIKE @Name";
                parameters.Add(Data.Db.Param("@Name", $"%{nameLike.Trim()}%"));
            }

            sql += " ORDER BY c.Name, p.FullName;";

            var table  = Data.Db.GetDataTable(sql, parameters.ToArray());
            var result = new List<Player>();

            foreach (DataRow row in table.Rows)
                result.Add(MapRow(row));

            return result;
        }

        // ── ADD ──────────────────────────────────────────────────────────────────
        public void Add(Player p)
        {
            const string sql = @"
                INSERT INTO players (ClubId, FullName, BirthDate, Position, ShirtNumber, Status)
                VALUES (@ClubId, @FullName, @BirthDate, @Position, @ShirtNumber, @Status);";

            Data.Db.ExecuteNonQuery(sql,
                Data.Db.Param("@ClubId",      p.ClubId),
                Data.Db.Param("@FullName",    p.FullName.Trim()),
                Data.Db.Param("@BirthDate",   p.BirthDate.ToString("yyyy-MM-dd")),
                Data.Db.Param("@Position",    p.Position),
                Data.Db.Param("@ShirtNumber", p.ShirtNumber.HasValue ? (object)p.ShirtNumber.Value : DBNull.Value),
                Data.Db.Param("@Status",      p.Status));
        }

        // ── UPDATE ───────────────────────────────────────────────────────────────
        public void Update(Player p)
        {
            const string sql = @"
                UPDATE players
                SET    ClubId      = @ClubId,
                       FullName    = @FullName,
                       BirthDate   = @BirthDate,
                       Position    = @Position,
                       ShirtNumber = @ShirtNumber,
                       Status      = @Status
                WHERE  PlayerId    = @PlayerId;";

            Data.Db.ExecuteNonQuery(sql,
                Data.Db.Param("@ClubId",      p.ClubId),
                Data.Db.Param("@FullName",    p.FullName.Trim()),
                Data.Db.Param("@BirthDate",   p.BirthDate.ToString("yyyy-MM-dd")),
                Data.Db.Param("@Position",    p.Position),
                Data.Db.Param("@ShirtNumber", p.ShirtNumber.HasValue ? (object)p.ShirtNumber.Value : DBNull.Value),
                Data.Db.Param("@Status",      p.Status),
                Data.Db.Param("@PlayerId",    p.PlayerId));
        }

        // ── DELETE ───────────────────────────────────────────────────────────────
        public void Delete(int playerId)
        {
            const string sql = "DELETE FROM players WHERE PlayerId = @PlayerId;";
            Data.Db.ExecuteNonQuery(sql, Data.Db.Param("@PlayerId", playerId));
        }

        // ── MAP ──────────────────────────────────────────────────────────────────
        private static Player MapRow(DataRow row) => new Player
        {
            PlayerId    = Convert.ToInt32(row["PlayerId"]),
            ClubId      = Convert.ToInt32(row["ClubId"]),
            ClubName    = row["ClubName"]?.ToString()  ?? "",
            FullName    = row["FullName"]?.ToString()  ?? "",
            BirthDate   = Convert.ToDateTime(row["BirthDate"]),
            Position    = row["Position"]?.ToString()  ?? "",
            ShirtNumber = row["ShirtNumber"] is DBNull ? null : Convert.ToInt32(row["ShirtNumber"]),
            Status      = row["Status"]?.ToString()    ?? "Active"
        };
    }
}
