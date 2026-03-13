using FootballLeague.Data;
using FootballLeague.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace FootballLeague.Repositories
{
    /// <summary>
    /// Репозитори за трансфери.
    /// AddTransfer изпълнява INSERT + UPDATE в една транзакция.
    /// </summary>
    internal class TransfersRepository
    {
        // ── LIST + FILTER ────────────────────────────────────────────────────────
        public List<Transfer> GetTransfers(int? playerId = null, int? clubId = null,
                                           DateTime? fromDate = null, DateTime? toDate = null)
        {
            var sql = @"
                SELECT  t.TransferId,
                        t.PlayerId,
                        p.FullName          AS PlayerName,
                        t.FromClubId,
                        IFNULL(fc.Name, 'Свободен агент') AS FromClubName,
                        t.ToClubId,
                        tc.Name             AS ToClubName,
                        t.TransferDate,
                        t.Fee,
                        t.Note
                FROM    transfers t
                JOIN    players p  ON p.PlayerId  = t.PlayerId
                LEFT JOIN clubs fc ON fc.ClubId   = t.FromClubId
                JOIN    clubs tc   ON tc.ClubId   = t.ToClubId
                WHERE   1=1";

            var prms = new List<MySqlParameter>();

            if (playerId.HasValue && playerId > 0)
            {
                sql += " AND t.PlayerId = @PlayerId";
                prms.Add(Db.Param("@PlayerId", playerId.Value));
            }
            if (clubId.HasValue && clubId > 0)
            {
                sql += " AND (t.FromClubId = @ClubId OR t.ToClubId = @ClubId)";
                prms.Add(Db.Param("@ClubId", clubId.Value));
            }
            if (fromDate.HasValue)
            {
                sql += " AND t.TransferDate >= @FromDate";
                prms.Add(Db.Param("@FromDate", fromDate.Value.ToString("yyyy-MM-dd")));
            }
            if (toDate.HasValue)
            {
                sql += " AND t.TransferDate <= @ToDate";
                prms.Add(Db.Param("@ToDate", toDate.Value.ToString("yyyy-MM-dd")));
            }

            sql += " ORDER BY t.TransferDate DESC;";

            var table  = Db.GetDataTable(sql, prms.ToArray());
            var result = new List<Transfer>();
            foreach (DataRow row in table.Rows)
                result.Add(MapRow(row));
            return result;
        }

        // ── ADD (в транзакция) ───────────────────────────────────────────────────
        /// <summary>
        /// Записва трансфера И ъпдейтва players.ClubId в една транзакция.
        /// При грешка → rollback.
        /// </summary>
        public void AddTransfer(Transfer t)
        {
            using var conn = Db.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                // 1) INSERT в transfers
                const string insertSql = @"
                    INSERT INTO transfers (PlayerId, FromClubId, ToClubId, TransferDate, Fee, Note)
                    VALUES (@PlayerId, @FromClubId, @ToClubId, @TransferDate, @Fee, @Note);";

                using (var cmd = new MySqlCommand(insertSql, conn, tx))
                {
                    cmd.Parameters.AddRange(new[]
                    {
                        Db.Param("@PlayerId",     t.PlayerId),
                        Db.Param("@FromClubId",   t.FromClubId.HasValue ? (object)t.FromClubId.Value : DBNull.Value),
                        Db.Param("@ToClubId",     t.ToClubId),
                        Db.Param("@TransferDate", t.TransferDate.ToString("yyyy-MM-dd")),
                        Db.Param("@Fee",          t.Fee.HasValue ? (object)t.Fee.Value : DBNull.Value),
                        Db.Param("@Note",         t.Note.Trim())
                    });
                    cmd.ExecuteNonQuery();
                }

                // 2) UPDATE players.ClubId
                const string updateSql = @"
                    UPDATE players SET ClubId = @ToClubId WHERE PlayerId = @PlayerId;";

                using (var cmd = new MySqlCommand(updateSql, conn, tx))
                {
                    cmd.Parameters.AddRange(new[]
                    {
                        Db.Param("@ToClubId",  t.ToClubId),
                        Db.Param("@PlayerId",  t.PlayerId)
                    });
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ── MAP ──────────────────────────────────────────────────────────────────
        private static Transfer MapRow(DataRow row) => new Transfer
        {
            TransferId   = Convert.ToInt32(row["TransferId"]),
            PlayerId     = Convert.ToInt32(row["PlayerId"]),
            PlayerName   = row["PlayerName"]?.ToString()  ?? "",
            FromClubId   = row["FromClubId"] is DBNull ? null : Convert.ToInt32(row["FromClubId"]),
            FromClubName = row["FromClubName"]?.ToString() ?? "Свободен агент",
            ToClubId     = Convert.ToInt32(row["ToClubId"]),
            ToClubName   = row["ToClubName"]?.ToString()  ?? "",
            TransferDate = Convert.ToDateTime(row["TransferDate"]),
            Fee          = row["Fee"] is DBNull ? null : Convert.ToDecimal(row["Fee"]),
            Note         = row["Note"]?.ToString() ?? ""
        };
    }
}
