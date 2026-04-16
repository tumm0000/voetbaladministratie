using Microsoft.Data.Sqlite;

namespace VoetbalAdministratie.Web.Data;

public sealed class DbInitializer
{
    private readonly Db _db;

    public DbInitializer(Db db)
    {
        _db = db;
    }

    public void EnsureCreatedAndSeeded()
    {
        using var connection = _db.OpenConnection();

        using (var pragma = connection.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();
        }

        CreateTables(connection);
        UpgradeSchema(connection);
        SeedIfEmpty(connection);
    }

    private static void CreateTables(SqliteConnection connection)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Lid (
                lid_id INTEGER PRIMARY KEY AUTOINCREMENT,
                voornaam TEXT NOT NULL,
                achternaam TEXT NOT NULL,
                email TEXT NOT NULL,
                telefoon TEXT NULL,
                geboortedatum TEXT NULL,
                lidmaatschap_type TEXT NOT NULL,
                status TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Team (
                team_id INTEGER PRIMARY KEY AUTOINCREMENT,
                naam TEXT NOT NULL,
                categorie TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS LidTeam (
                lid_team_id INTEGER PRIMARY KEY AUTOINCREMENT,
                lid_id INTEGER NOT NULL,
                team_id INTEGER NOT NULL,
                vanaf_datum TEXT NOT NULL,
                CONSTRAINT fk_lidteam_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id),
                CONSTRAINT fk_lidteam_team FOREIGN KEY (team_id) REFERENCES Team(team_id)
            );

            CREATE TABLE IF NOT EXISTS Contributie (
                contributie_id INTEGER PRIMARY KEY AUTOINCREMENT,
                lid_id INTEGER NOT NULL,
                bedrag REAL NOT NULL CHECK (bedrag > 0),
                vervaldatum TEXT NOT NULL,
                status TEXT NOT NULL,
                CONSTRAINT fk_contributie_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id)
            );

            CREATE TABLE IF NOT EXISTS Betaling (
                betaling_id INTEGER PRIMARY KEY AUTOINCREMENT,
                contributie_id INTEGER NOT NULL,
                bedrag REAL NOT NULL CHECK (bedrag > 0),
                betaaldatum TEXT NOT NULL,
                betaalmethode TEXT NULL,
                CONSTRAINT fk_betaling_contributie FOREIGN KEY (contributie_id) REFERENCES Contributie(contributie_id)
            );

            CREATE TABLE IF NOT EXISTS Veld (
                veld_id INTEGER PRIMARY KEY AUTOINCREMENT,
                naam TEXT NOT NULL,
                locatie TEXT NULL
            );

            CREATE TABLE IF NOT EXISTS Beschikbaarheid (
                beschikbaarheid_id INTEGER PRIMARY KEY AUTOINCREMENT,
                lid_id INTEGER NOT NULL,
                datum TEXT NOT NULL,
                tijdslot TEXT NOT NULL,
                status TEXT NOT NULL,
                CONSTRAINT fk_beschikbaarheid_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id)
            );

            CREATE TABLE IF NOT EXISTS Wedstrijd (
                wedstrijd_id INTEGER PRIMARY KEY AUTOINCREMENT,
                team_id INTEGER NOT NULL,
                veld_id INTEGER NOT NULL,
                datum TEXT NOT NULL,
                tijdslot TEXT NOT NULL,
                tegenstander TEXT NULL,
                status TEXT NOT NULL,
                CONSTRAINT fk_wedstrijd_team FOREIGN KEY (team_id) REFERENCES Team(team_id),
                CONSTRAINT fk_wedstrijd_veld FOREIGN KEY (veld_id) REFERENCES Veld(veld_id)
            );

            CREATE TABLE IF NOT EXISTS Training (
                training_id INTEGER PRIMARY KEY AUTOINCREMENT,
                team_id INTEGER NOT NULL,
                veld_id INTEGER NOT NULL,
                datum TEXT NOT NULL,
                tijdslot TEXT NOT NULL,
                status TEXT NOT NULL,
                CONSTRAINT fk_training_team FOREIGN KEY (team_id) REFERENCES Team(team_id),
                CONSTRAINT fk_training_veld FOREIGN KEY (veld_id) REFERENCES Veld(veld_id)
            );
            """;
        cmd.ExecuteNonQuery();
    }

    private static void UpgradeSchema(SqliteConnection connection)
    {
        if (!ColumnExists(connection, "Lid", "geboortedatum"))
        {
            using var alter = connection.CreateCommand();
            alter.CommandText = "ALTER TABLE Lid ADD COLUMN geboortedatum TEXT NULL;";
            alter.ExecuteNonQuery();
        }

        BackfillLidGeboortedatumIfNeeded(connection);
    }

    private static bool ColumnExists(SqliteConnection connection, string table, string column)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info({table});";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            // cid, name, type, notnull, dflt_value, pk
            var name = reader.GetString(1);
            if (string.Equals(name, column, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    }

    private static void BackfillLidGeboortedatumIfNeeded(SqliteConnection connection)
    {
        using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(1) FROM Lid WHERE geboortedatum IS NULL;";
        var missing = Convert.ToInt32(check.ExecuteScalar());
        if (missing == 0) return;

        // Realistische demo-geboortedata (o.a. voor senioren/jeugd checks bij teamkoppeling)
        var map = new Dictionary<int, string>
        {
            [1] = "1998-05-12",
            [2] = "2000-09-03",
            [3] = "2010-02-18",
            [4] = "2011-11-07",
            [5] = "1999-01-25",
            [6] = "2001-07-30",
        };

        foreach (var (lidId, geboortedatum) in map)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText =
                """
                UPDATE Lid
                SET geboortedatum = $geboortedatum
                WHERE lid_id = $lidId AND geboortedatum IS NULL;
                """;
            cmd.Parameters.AddWithValue("$geboortedatum", geboortedatum);
            cmd.Parameters.AddWithValue("$lidId", lidId);
            cmd.ExecuteNonQuery();
        }

        using var fallback = connection.CreateCommand();
        fallback.CommandText =
            """
            UPDATE Lid
            SET geboortedatum = CASE
                WHEN (lid_id % 2) = 0 THEN '2011-06-01'
                ELSE '1998-06-01'
            END
            WHERE geboortedatum IS NULL;
            """;
        fallback.ExecuteNonQuery();
    }

    private static void SeedIfEmpty(SqliteConnection connection)
    {
        using var countCmd = connection.CreateCommand();
        countCmd.CommandText = "SELECT COUNT(1) FROM Lid;";
        var existing = Convert.ToInt32(countCmd.ExecuteScalar());
        if (existing > 0) return;

        using var tx = connection.BeginTransaction();

        InsertTeams(connection, tx);
        InsertVelden(connection, tx);
        InsertLeden(connection, tx);
        InsertLidTeam(connection, tx);
        InsertBeschikbaarheid(connection, tx);
        InsertContributiesEnBetalingen(connection, tx);
        InsertEersteWedstrijden(connection, tx);

        tx.Commit();
    }

    private static void InsertTeams(SqliteConnection connection, SqliteTransaction tx)
    {
        var teams = new[]
        {
            ("JO17-1", "Jeugd"),
            ("Heren 1", "Senioren"),
            ("Dames 1", "Senioren"),
        };

        foreach (var (naam, categorie) in teams)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT INTO Team (naam, categorie) VALUES ($naam, $categorie);";
            cmd.Parameters.AddWithValue("$naam", naam);
            cmd.Parameters.AddWithValue("$categorie", categorie);
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertVelden(SqliteConnection connection, SqliteTransaction tx)
    {
        var velden = new[]
        {
            ("Veld 1", "Sportpark De Bosrand"),
            ("Veld 2", "Sportpark De Bosrand"),
        };

        foreach (var (naam, locatie) in velden)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT INTO Veld (naam, locatie) VALUES ($naam, $locatie);";
            cmd.Parameters.AddWithValue("$naam", naam);
            cmd.Parameters.AddWithValue("$locatie", locatie);
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertLeden(SqliteConnection connection, SqliteTransaction tx)
    {
        var leden = new[]
        {
            ("Sam", "Jansen", "sam.jansen@club.nl", "0612345678", "1998-05-12", "Spelend", "Actief"),
            ("Noa", "de Vries", "noa.devries@club.nl", "0687654321", "2000-09-03", "Spelend", "Actief"),
            ("Milan", "Bakker", "milan.bakker@club.nl", null, "2010-02-18", "Spelend", "Actief"),
            ("Sara", "Visser", "sara.visser@club.nl", null, "2011-11-07", "Spelend", "Actief"),
            ("Daan", "Smit", "daan.smit@club.nl", "0611122233", "1999-01-25", "Niet-spelend", "Actief"),
            ("Lotte", "Bos", "lotte.bos@club.nl", "0699988877", "2001-07-30", "Spelend", "Actief"),
        };

        foreach (var (voornaam, achternaam, email, telefoon, geboortedatum, type, status) in leden)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText =
                """
                INSERT INTO Lid (voornaam, achternaam, email, telefoon, geboortedatum, lidmaatschap_type, status)
                VALUES ($voornaam, $achternaam, $email, $telefoon, $geboortedatum, $type, $status);
                """;
            cmd.Parameters.AddWithValue("$voornaam", voornaam);
            cmd.Parameters.AddWithValue("$achternaam", achternaam);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$telefoon", (object?)telefoon ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$geboortedatum", geboortedatum);
            cmd.Parameters.AddWithValue("$type", type);
            cmd.Parameters.AddWithValue("$status", status);
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertLidTeam(SqliteConnection connection, SqliteTransaction tx)
    {
        // Lid IDs: 1..6; Team IDs: 1..3
        var koppelingen = new[]
        {
            (1, 2),
            (2, 2),
            (3, 1),
            (4, 1),
            (5, 3),
            (6, 3),
        };

        foreach (var (lidId, teamId) in koppelingen)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText =
                """
                INSERT INTO LidTeam (lid_id, team_id, vanaf_datum)
                VALUES ($lidId, $teamId, $vanaf);
                """;
            cmd.Parameters.AddWithValue("$lidId", lidId);
            cmd.Parameters.AddWithValue("$teamId", teamId);
            cmd.Parameters.AddWithValue("$vanaf", DateTime.UtcNow.Date.AddMonths(-2).ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertBeschikbaarheid(SqliteConnection connection, SqliteTransaction tx)
    {
        var tijdsloten = new[] { "18:00-19:30", "19:30-21:00" };
        var start = DateTime.UtcNow.Date.AddDays(1);

        for (var day = 0; day < 14; day++)
        {
            var datum = start.AddDays(day).ToString("yyyy-MM-dd");

            for (var lidId = 1; lidId <= 6; lidId++)
            {
                foreach (var tijdslot in tijdsloten)
                {
                    var status = (lidId + day) % 5 == 0 ? "Onbeschikbaar" : "Beschikbaar";
                    using var cmd = connection.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText =
                        """
                        INSERT INTO Beschikbaarheid (lid_id, datum, tijdslot, status)
                        VALUES ($lidId, $datum, $tijdslot, $status);
                        """;
                    cmd.Parameters.AddWithValue("$lidId", lidId);
                    cmd.Parameters.AddWithValue("$datum", datum);
                    cmd.Parameters.AddWithValue("$tijdslot", tijdslot);
                    cmd.Parameters.AddWithValue("$status", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    private static void InsertContributiesEnBetalingen(SqliteConnection connection, SqliteTransaction tx)
    {
        var today = DateTime.UtcNow.Date;
        for (var lidId = 1; lidId <= 6; lidId++)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText =
                """
                INSERT INTO Contributie (lid_id, bedrag, vervaldatum, status)
                VALUES ($lidId, $bedrag, $verval, $status);
                """;
            cmd.Parameters.AddWithValue("$lidId", lidId);
            cmd.Parameters.AddWithValue("$bedrag", 25.00m);
            cmd.Parameters.AddWithValue("$verval", today.AddDays(-10).ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$status", lidId % 3 == 0 ? "Open" : "Betaald");
            cmd.ExecuteNonQuery();

            using var lastIdCmd = connection.CreateCommand();
            lastIdCmd.Transaction = tx;
            lastIdCmd.CommandText = "SELECT last_insert_rowid();";
            var contributieId = (long)(lastIdCmd.ExecuteScalar() ?? 0L);
            if (lidId % 3 != 0)
            {
                using var pay = connection.CreateCommand();
                pay.Transaction = tx;
                pay.CommandText =
                    """
                    INSERT INTO Betaling (contributie_id, bedrag, betaaldatum, betaalmethode)
                    VALUES ($contributieId, $bedrag, $datum, $methode);
                    """;
                pay.Parameters.AddWithValue("$contributieId", contributieId);
                pay.Parameters.AddWithValue("$bedrag", 25.00m);
                pay.Parameters.AddWithValue("$datum", today.AddDays(-7).ToString("yyyy-MM-dd"));
                pay.Parameters.AddWithValue("$methode", "Contant");
                pay.ExecuteNonQuery();
            }
        }
    }

    private static void InsertEersteWedstrijden(SqliteConnection connection, SqliteTransaction tx)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText =
            """
            INSERT INTO Wedstrijd (team_id, veld_id, datum, tijdslot, tegenstander, status)
            VALUES (2, 1, $datum, '19:30-21:00', 'FC Voorbeeld', 'Gepland');
            """;
        cmd.Parameters.AddWithValue("$datum", DateTime.UtcNow.Date.AddDays(3).ToString("yyyy-MM-dd"));
        cmd.ExecuteNonQuery();
    }
}

