# Football League – CRUD Клубове (Етап 2)

## Изисквания
- Windows 10/11
- Visual Studio 2022 (или по-нова)
- XAMPP с активиран MySQL (порт 3306)
- .NET 6 SDK

---

## 1. Настройка на базата данни

1. Стартирай XAMPP и пусни **MySQL**.
2. Отвори **phpMyAdmin** → `http://localhost/phpmyadmin`
3. Изпълни файла **`schema.sql`** (Import → избери файла).
4. След това изпълни **`seed.sql`** за примерни данни.

---

## 2. Настройка на Connection String

Отвори `FootballLeague/App.config` и провери реда:

```xml
<add name="FootballDB"
     connectionString="server=localhost;port=3306;database=football_league;uid=root;pwd=;charset=utf8mb4;SslMode=none;"
     providerName="MySql.Data.MySqlClient" />
```

| Параметър  | Стойност по подразбиране | Промени при нужда |
|------------|--------------------------|-------------------|
| `server`   | `localhost`              | ако MySQL е на друга машина |
| `port`     | `3306`                   | ако XAMPP ползва друг порт |
| `database` | `football_league`        | не променяй |
| `uid`      | `root`                   | твоят MySQL потребител |
| `pwd`      | _(празно)_               | твоята MySQL парола |

---

## 3. Стартиране

1. Отвори `FootballLeague.sln` с Visual Studio.
2. Изчакай NuGet да изтегли пакета `MySql.Data`.
3. Натисни **F5** или бутона ▶ Start.

---

## 4. Архитектура на проекта

```
FootballLeague/
├── Data/
│   ├── DbConfig.cs          ← чете connection string от App.config
│   └── Db.cs                ← GetConnection, ExecuteNonQuery, GetDataTable
├── Models/
│   └── Club.cs              ← модел (ClubId, Name, City, CreatedAt)
├── Repositories/
│   └── ClubsRepository.cs   ← GetAll, Add, Update, Delete (SQL тук)
├── Forms/
│   ├── ClubsForm.cs         ← UI логика
│   └── ClubsForm.Designer.cs← контроли и оформление
├── Program.cs
└── App.config               ← connection string
```

---

## 5. CRUD функционалности

| Операция | Как | SQL |
|----------|-----|-----|
| **List** | Автоматично при стартиране + бутон 🔄 | `SELECT … ORDER BY Name` |
| **Add**  | Попълни Name (и City), натисни ➕ | `INSERT INTO clubs …` |
| **Edit** | Избери ред → промени → натисни ✏️ | `UPDATE clubs SET … WHERE ClubId=?` |
| **Delete** | Избери ред → натисни 🗑️ → потвърди | `DELETE FROM clubs WHERE ClubId=?` |

Всички заявки са **параметризирани** (без string concat).
