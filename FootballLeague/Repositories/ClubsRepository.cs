using FootballLeague.Data;
using FootballLeague.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace FootballLeague.Repositories
{
    /// <summary>
    /// Репозитори за клубове — всички SQL заявки са тук.
    /// UI кодът НЕ съдържа SQL.
    /// </summary>
    internal class ClubsRepository
    {
        // ── LIST ────────────────────────────────────────────────────────────────
        /// <summary>Връща всички клубове, сортирани по Name.</summary>
        public List<Club> GetAll()
        {
            const string sql = @"
                SELECT ClubId, Name, City, CreatedAt
                FROM   clubs
                ORDER BY Name;";

            var table  = Db.GetDataTable(sql);
            var result = new List<Club>();

            foreach (DataRow row in table.Rows)
            {
                result.Add(MapRow(row));
            }
            return result;
        }

        // ── ADD ─────────────────────────────────────────────────────────────────
        /// <summary>Добавя нов клуб. Хвърля изключение при дублирано Name.</summary>
        public void Add(Club club)
        {
            const string sql = @"
                INSERT INTO clubs (Name, City, CreatedAt)
                VALUES (@Name, @City, NOW());";

            Db.ExecuteNonQuery(sql,
                Db.Param("@Name", club.Name.Trim()),
                Db.Param("@City", club.City.Trim()));
        }

        // ── EDIT ────────────────────────────────────────────────────────────────
        /// <summary>Обновява Name и City по ClubId.</summary>
        public void Update(Club club)
        {
            const string sql = @"
                UPDATE clubs
                SET    Name = @Name,
                       City = @City
                WHERE  ClubId = @ClubId;";

            Db.ExecuteNonQuery(sql,
                Db.Param("@Name",   club.Name.Trim()),
                Db.Param("@City",   club.City.Trim()),
                Db.Param("@ClubId", club.ClubId));
        }

        // ── DELETE ──────────────────────────────────────────────────────────────
        /// <summary>Изтрива клуб по ClubId.</summary>
        public void Delete(int clubId)
        {
            const string sql = "DELETE FROM clubs WHERE ClubId = @ClubId;";
            Db.ExecuteNonQuery(sql, Db.Param("@ClubId", clubId));
        }

        // ── Helpers ─────────────────────────────────────────────────────────────
        private static Club MapRow(DataRow row) => new Club
        {
            ClubId    = Convert.ToInt32(row["ClubId"]),
            Name      = row["Name"]?.ToString()   ?? "",
            City      = row["City"]?.ToString()   ?? "",
            CreatedAt = row["CreatedAt"] is DBNull ? null : Convert.ToDateTime(row["CreatedAt"])
        };
    }
}
