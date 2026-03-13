using System.Configuration;

namespace FootballLeague.Data
{
    /// <summary>
    /// Чете connection string от App.config.
    /// Ключът "FootballDB" трябва да съществува в &lt;connectionStrings&gt;.
    /// </summary>
    internal static class DbConfig
    {
        public static string ConnectionString
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["FootballDB"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException(
                        "Не е намерен connection string 'FootballDB' в App.config.");
                return cs;
            }
        }
    }
}
