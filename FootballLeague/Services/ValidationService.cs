namespace FootballLeague.Services
{
    /// <summary>
    /// Централизирани валидации за играч.
    /// Връща null при успех, или съобщение за грешка.
    /// </summary>
    internal static class ValidationService
    {
        private static readonly string[] ValidPositions = { "GK", "DF", "MF", "FW" };
        private static readonly string[] ValidStatuses  = { "Active", "Injured", "Suspended" };

        public static string? ValidateFullName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Името не може да е празно.";
            if (name.Trim().Length < 3)
                return "Името трябва да е поне 3 символа.";
            if (name.Trim().All(c => !char.IsLetter(c)))
                return "Името трябва да съдържа поне една буква.";
            return null;
        }

        public static string? ValidateBirthDate(DateTime date)
        {
            if (date > DateTime.Today)
                return "Датата на раждане не може да е в бъдещето.";

            int age = DateTime.Today.Year - date.Year
                      - (DateTime.Today.DayOfYear < date.DayOfYear ? 1 : 0);

            if (age < 10)
                return $"Играчът е твърде млад ({age} г.). Минимална възраст: 10.";
            if (age > 60)
                return $"Невалидна дата на раждане ({age} г.). Максимална възраст: 60.";

            return null;
        }

        public static string? ValidatePosition(string position)
        {
            if (!ValidPositions.Contains(position))
                return $"Позицията трябва да е една от: {string.Join(", ", ValidPositions)}.";
            return null;
        }

        public static string? ValidateClub(int clubId)
        {
            if (clubId <= 0)
                return "Моля, изберете клуб.";
            return null;
        }

        /// <summary>Валидира всички полета и връща списък с грешки.</summary>
        public static List<string> ValidatePlayer(string fullName, DateTime birthDate,
                                                   string position, int clubId)
        {
            var errors = new List<string>();
            void Add(string? msg) { if (msg != null) errors.Add(msg); }

            Add(ValidateFullName(fullName));
            Add(ValidateBirthDate(birthDate));
            Add(ValidatePosition(position));
            Add(ValidateClub(clubId));

            return errors;
        }
    }
}
