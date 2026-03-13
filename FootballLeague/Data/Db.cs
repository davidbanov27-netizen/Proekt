using MySql.Data.MySqlClient;
using System.Data;

namespace FootballLeague.Data
{
    /// <summary>
    /// Слой за достъп до MySQL база данни.
    /// Всички методи използват параметризирани заявки и using блокове.
    /// </summary>
    internal static class Db
    {
        // ── Връзка ─────────────────────────────────────────────────────────────
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(DbConfig.ConnectionString);
        }

        // ── INSERT / UPDATE / DELETE ────────────────────────────────────────────
        /// <summary>
        /// Изпълнява не-SELECT заявка и връща броя засегнати редове.
        /// </summary>
        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using var conn = GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteNonQuery();
        }

        // ── SELECT → DataTable ──────────────────────────────────────────────────
        /// <summary>
        /// Изпълнява SELECT заявка и връща резултата като DataTable.
        /// </summary>
        public static DataTable GetDataTable(string sql, params MySqlParameter[] parameters)
        {
            using var conn = GetConnection();
            conn.Open();

            using var cmd  = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);

            using var adapter = new MySqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // ── SELECT → scalar ─────────────────────────────────────────────────────
        /// <summary>
        /// Връща единична стойност (напр. COUNT, последен ID).
        /// </summary>
        public static object? ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using var conn = GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteScalar();
        }

        // ── Помощен метод за параметър ──────────────────────────────────────────
        public static MySqlParameter Param(string name, object? value)
            => new MySqlParameter(name, value ?? DBNull.Value);
    }
}
