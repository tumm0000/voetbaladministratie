using Microsoft.Data.Sqlite;
using VoetbalAdministratie.Web.Data;
using VoetbalAdministratie.Web.Models;

namespace VoetbalAdministratie.Web.Repositories;

public sealed class LidRepository
{
    private readonly Db _db;

    public LidRepository(Db db)
    {
        _db = db;
    }

    public List<Lid> GetAll()
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT lid_id, voornaam, achternaam, email, telefoon, geboortedatum, lidmaatschap_type, status
            FROM Lid
            ORDER BY achternaam, voornaam;
            """;

        using var reader = cmd.ExecuteReader();
        var result = new List<Lid>();
        while (reader.Read())
        {
            result.Add(new Lid
            {
                LidId = reader.GetInt32(0),
                Voornaam = reader.GetString(1),
                Achternaam = reader.GetString(2),
                Email = reader.GetString(3),
                Telefoon = reader.IsDBNull(4) ? null : reader.GetString(4),
                Geboortedatum = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                LidmaatschapType = reader.GetString(6),
                Status = reader.GetString(7),
            });
        }

        return result;
    }

    public Lid? GetById(int lidId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT lid_id, voornaam, achternaam, email, telefoon, geboortedatum, lidmaatschap_type, status
            FROM Lid
            WHERE lid_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", lidId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Lid
        {
            LidId = reader.GetInt32(0),
            Voornaam = reader.GetString(1),
            Achternaam = reader.GetString(2),
            Email = reader.GetString(3),
            Telefoon = reader.IsDBNull(4) ? null : reader.GetString(4),
            Geboortedatum = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
            LidmaatschapType = reader.GetString(6),
            Status = reader.GetString(7),
        };
    }

    public int Insert(Lid lid)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            INSERT INTO Lid (voornaam, achternaam, email, telefoon, geboortedatum, lidmaatschap_type, status)
            VALUES ($voornaam, $achternaam, $email, $telefoon, $geboortedatum, $type, $status);
            """;
        cmd.Parameters.AddWithValue("$voornaam", lid.Voornaam);
        cmd.Parameters.AddWithValue("$achternaam", lid.Achternaam);
        cmd.Parameters.AddWithValue("$email", lid.Email);
        cmd.Parameters.AddWithValue("$telefoon", (object?)lid.Telefoon ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$geboortedatum", lid.Geboortedatum?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$type", lid.LidmaatschapType);
        cmd.Parameters.AddWithValue("$status", lid.Status);
        cmd.ExecuteNonQuery();

        using var idCmd = connection.CreateCommand();
        idCmd.CommandText = "SELECT last_insert_rowid();";
        return Convert.ToInt32(idCmd.ExecuteScalar());
    }

    public void Update(Lid lid)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            UPDATE Lid
            SET voornaam = $voornaam,
                achternaam = $achternaam,
                email = $email,
                telefoon = $telefoon,
                geboortedatum = $geboortedatum,
                lidmaatschap_type = $type,
                status = $status
            WHERE lid_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", lid.LidId);
        cmd.Parameters.AddWithValue("$voornaam", lid.Voornaam);
        cmd.Parameters.AddWithValue("$achternaam", lid.Achternaam);
        cmd.Parameters.AddWithValue("$email", lid.Email);
        cmd.Parameters.AddWithValue("$telefoon", (object?)lid.Telefoon ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$geboortedatum", lid.Geboortedatum?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$type", lid.LidmaatschapType);
        cmd.Parameters.AddWithValue("$status", lid.Status);
        cmd.ExecuteNonQuery();
    }
}

