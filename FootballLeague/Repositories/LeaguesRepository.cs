using FootballLeague.Data;
using FootballLeague.Models;
using System.Data;

namespace FootballLeague.Repositories
{
    internal class LeaguesRepository
    {
        // ── LIST ────────────────────────────────────────────────────────────────
        public List<League> GetAll()
        {
            const string sql = @"
                SELECT LeagueId, Name, Season
                FROM   leagues
                ORDER  BY Season DESC, Name;";

            var result = new List<League>();
            foreach (DataRow row in Db.GetDataTable(sql).Rows)
                result.Add(Map(row));
            return result;
        }

        // ── CREATE ───────────────────────────────────────────────────────────────
        public void Create(League l)
        {
            const string sql = @"
                INSERT INTO leagues (Name, Season)
                VALUES (@Name, @Season);";

            Db.ExecuteNonQuery(sql,
                Db.Param("@Name",   l.Name.Trim()),
                Db.Param("@Season", l.Season.Trim()));
        }

        // ── UPDATE ───────────────────────────────────────────────────────────────
        public void Update(League l)
        {
            const string sql = @"
                UPDATE leagues
                SET    Name   = @Name,
                       Season = @Season
                WHERE  LeagueId = @LeagueId;";

            Db.ExecuteNonQuery(sql,
                Db.Param("@Name",     l.Name.Trim()),
                Db.Param("@Season",   l.Season.Trim()),
                Db.Param("@LeagueId", l.LeagueId));
        }

        // ── DELETE ───────────────────────────────────────────────────────────────
        public void Delete(int leagueId)
        {
            const string sql = "DELETE FROM leagues WHERE LeagueId = @LeagueId;";
            Db.ExecuteNonQuery(sql, Db.Param("@LeagueId", leagueId));
        }

        // ── Has participants ─────────────────────────────────────────────────────
        public bool HasParticipants(int leagueId)
        {
            const string sql = @"
                SELECT COUNT(*) FROM league_teams WHERE LeagueId = @LeagueId;";
            var val = Db.ExecuteScalar(sql, Db.Param("@LeagueId", leagueId));
            return Convert.ToInt32(val) > 0;
        }

        private static League Map(DataRow r) => new League
        {
            LeagueId = Convert.ToInt32(r["LeagueId"]),
            Name     = r["Name"]?.ToString()   ?? "",
            Season   = r["Season"]?.ToString() ?? ""
        };
    }
}
