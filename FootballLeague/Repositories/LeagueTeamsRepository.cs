using FootballLeague.Data;
using FootballLeague.Models;
using System.Data;

namespace FootballLeague.Repositories
{
    internal class LeagueTeamsRepository
    {
        // ── Участници в лига ─────────────────────────────────────────────────────
        public List<Club> GetParticipants(int leagueId)
        {
            const string sql = @"
                SELECT c.ClubId, c.Name, c.City, c.CreatedAt
                FROM   league_teams lt
                JOIN   clubs        c  ON c.ClubId = lt.ClubId
                WHERE  lt.LeagueId = @LeagueId
                ORDER  BY c.Name;";

            var result = new List<Club>();
            foreach (DataRow row in Db.GetDataTable(sql, Db.Param("@LeagueId", leagueId)).Rows)
                result.Add(MapClub(row));
            return result;
        }

        // ── Налични клубове (не са участници в тази лига) ────────────────────────
        public List<Club> GetAvailableClubs(int leagueId)
        {
            const string sql = @"
                SELECT c.ClubId, c.Name, c.City, c.CreatedAt
                FROM   clubs c
                LEFT JOIN league_teams lt
                       ON lt.ClubId   = c.ClubId
                      AND lt.LeagueId = @LeagueId
                WHERE  lt.ClubId IS NULL
                ORDER  BY c.Name;";

            var result = new List<Club>();
            foreach (DataRow row in Db.GetDataTable(sql, Db.Param("@LeagueId", leagueId)).Rows)
                result.Add(MapClub(row));
            return result;
        }

        // ── Добави клуб в лига ───────────────────────────────────────────────────
        public void AddClub(int leagueId, int clubId)
        {
            const string sql = @"
                INSERT INTO league_teams (LeagueId, ClubId)
                VALUES (@LeagueId, @ClubId);";

            Db.ExecuteNonQuery(sql,
                Db.Param("@LeagueId", leagueId),
                Db.Param("@ClubId",   clubId));
        }

        // ── Премахни клуб от лига ────────────────────────────────────────────────
        public void RemoveClub(int leagueId, int clubId)
        {
            const string sql = @"
                DELETE FROM league_teams
                WHERE LeagueId = @LeagueId AND ClubId = @ClubId;";

            Db.ExecuteNonQuery(sql,
                Db.Param("@LeagueId", leagueId),
                Db.Param("@ClubId",   clubId));
        }

        private static Club MapClub(DataRow r) => new Club
        {
            ClubId    = Convert.ToInt32(r["ClubId"]),
            Name      = r["Name"]?.ToString() ?? "",
            City      = r["City"]?.ToString() ?? "",
            CreatedAt = r["CreatedAt"] is DBNull ? null : Convert.ToDateTime(r["CreatedAt"])
        };
    }
}
