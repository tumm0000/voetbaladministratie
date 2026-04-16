using VoetbalAdministratie.Web.Data;

namespace VoetbalAdministratie.Web.Repositories;

public sealed record BetalingItem(
    int ContributieId,
    int LidId,
    string Naam,
    string Email,
    decimal Bedrag,
    DateTime Vervaldatum,
    string Status
);

public sealed class BetalingRepository
{
    private readonly Db _db;

    public BetalingRepository(Db db)
    {
        _db = db;
    }

    public List<BetalingItem> GetBetalingen()
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT
                c.contributie_id,
                l.lid_id,
                (l.voornaam || ' ' || l.achternaam) AS naam,
                l.email,
                c.bedrag,
                c.vervaldatum,
                c.status
            FROM Contributie c
            JOIN Lid l ON l.lid_id = c.lid_id
            ORDER BY
                CASE WHEN c.status = 'Open' THEN 0 ELSE 1 END ASC,
                c.vervaldatum ASC;
            """;

        using var reader = cmd.ExecuteReader();
        var result = new List<BetalingItem>();
        while (reader.Read())
        {
            result.Add(new BetalingItem(
                ContributieId: reader.GetInt32(0),
                LidId: reader.GetInt32(1),
                Naam: reader.GetString(2),
                Email: reader.GetString(3),
                Bedrag: Convert.ToDecimal(reader.GetDouble(4)),
                Vervaldatum: DateTime.Parse(reader.GetString(5)),
                Status: reader.GetString(6)
            ));
        }

        return result;
    }

    public (int contributieId, int lidId, decimal bedrag, DateTime vervaldatum, string status)? GetContributie(int contributieId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT contributie_id, lid_id, bedrag, vervaldatum, status
            FROM Contributie
            WHERE contributie_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", contributieId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return (
            contributieId: reader.GetInt32(0),
            lidId: reader.GetInt32(1),
            bedrag: Convert.ToDecimal(reader.GetDouble(2)),
            vervaldatum: DateTime.Parse(reader.GetString(3)),
            status: reader.GetString(4)
        );
    }

    public int InsertContributie(int lidId, decimal bedrag, DateTime vervaldatum, string status)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            INSERT INTO Contributie (lid_id, bedrag, vervaldatum, status)
            VALUES ($lidId, $bedrag, $verval, $status);
            """;
        cmd.Parameters.AddWithValue("$lidId", lidId);
        cmd.Parameters.AddWithValue("$bedrag", bedrag);
        cmd.Parameters.AddWithValue("$verval", vervaldatum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$status", status);
        cmd.ExecuteNonQuery();

        using var idCmd = connection.CreateCommand();
        idCmd.CommandText = "SELECT last_insert_rowid();";
        return Convert.ToInt32(idCmd.ExecuteScalar());
    }

    public void UpdateContributie(int contributieId, int lidId, decimal bedrag, DateTime vervaldatum, string status)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            UPDATE Contributie
            SET lid_id = $lidId,
                bedrag = $bedrag,
                vervaldatum = $verval,
                status = $status
            WHERE contributie_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", contributieId);
        cmd.Parameters.AddWithValue("$lidId", lidId);
        cmd.Parameters.AddWithValue("$bedrag", bedrag);
        cmd.Parameters.AddWithValue("$verval", vervaldatum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$status", status);
        cmd.ExecuteNonQuery();
    }

    public void DeleteContributie(int contributieId)
    {
        using var connection = _db.OpenConnection();
        using var tx = connection.BeginTransaction();

        using (var delBetalingen = connection.CreateCommand())
        {
            delBetalingen.Transaction = tx;
            delBetalingen.CommandText = "DELETE FROM Betaling WHERE contributie_id = $id;";
            delBetalingen.Parameters.AddWithValue("$id", contributieId);
            delBetalingen.ExecuteNonQuery();
        }

        using (var delContributie = connection.CreateCommand())
        {
            delContributie.Transaction = tx;
            delContributie.CommandText = "DELETE FROM Contributie WHERE contributie_id = $id;";
            delContributie.Parameters.AddWithValue("$id", contributieId);
            delContributie.ExecuteNonQuery();
        }

        tx.Commit();
    }

    public void InsertBetaling(int contributieId, decimal bedrag, DateTime betaaldatum, string betaalmethode)
    {
        using var connection = _db.OpenConnection();
        using var tx = connection.BeginTransaction();

        using (var cmd = connection.CreateCommand())
        {
            cmd.Transaction = tx;
            cmd.CommandText =
                """
                INSERT INTO Betaling (contributie_id, bedrag, betaaldatum, betaalmethode)
                VALUES ($contributieId, $bedrag, $betaaldatum, $methode);
                """;
            cmd.Parameters.AddWithValue("$contributieId", contributieId);
            cmd.Parameters.AddWithValue("$bedrag", bedrag);
            cmd.Parameters.AddWithValue("$betaaldatum", betaaldatum.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$methode", betaalmethode);
            cmd.ExecuteNonQuery();
        }

        using (var update = connection.CreateCommand())
        {
            update.Transaction = tx;
            update.CommandText = "UPDATE Contributie SET status = 'Betaald' WHERE contributie_id = $id;";
            update.Parameters.AddWithValue("$id", contributieId);
            update.ExecuteNonQuery();
        }

        tx.Commit();
    }
}

