using VoetbalAdministratie.Web.Data;
using VoetbalAdministratie.Web.Models;

namespace VoetbalAdministratie.Web.Repositories;

public sealed record PlanningItem(
    int WedstrijdId,
    string TeamNaam,
    string VeldNaam,
    DateTime Datum,
    string Tijdslot,
    string? Tegenstander,
    string Status
);

public sealed class PlanningRepository
{
    private readonly Db _db;

    public PlanningRepository(Db db)
    {
        _db = db;
    }

    public List<PlanningItem> GetPlanning()
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT
                w.wedstrijd_id,
                t.naam AS team_naam,
                v.naam AS veld_naam,
                w.datum,
                w.tijdslot,
                w.tegenstander,
                w.status
            FROM Wedstrijd w
            JOIN Team t ON t.team_id = w.team_id
            JOIN Veld v ON v.veld_id = w.veld_id
            ORDER BY w.datum ASC, w.tijdslot ASC;
            """;

        using var reader = cmd.ExecuteReader();
        var result = new List<PlanningItem>();
        while (reader.Read())
        {
            result.Add(new PlanningItem(
                WedstrijdId: reader.GetInt32(0),
                TeamNaam: reader.GetString(1),
                VeldNaam: reader.GetString(2),
                Datum: DateTime.Parse(reader.GetString(3)),
                Tijdslot: reader.GetString(4),
                Tegenstander: reader.IsDBNull(5) ? null : reader.GetString(5),
                Status: reader.GetString(6)
            ));
        }

        return result;
    }

    public bool BestaatVeldConflict(int veldId, DateTime datum, string tijdslot, int? excludeWedstrijdId = null)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            excludeWedstrijdId is null
                ? """
                  SELECT COUNT(1)
                  FROM Wedstrijd
                  WHERE veld_id = $veldId AND datum = $datum AND tijdslot = $tijdslot;
                  """
                : """
                  SELECT COUNT(1)
                  FROM Wedstrijd
                  WHERE veld_id = $veldId
                    AND datum = $datum
                    AND tijdslot = $tijdslot
                    AND wedstrijd_id <> $excludeId;
                  """;
        cmd.Parameters.AddWithValue("$veldId", veldId);
        cmd.Parameters.AddWithValue("$datum", datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$tijdslot", tijdslot);
        if (excludeWedstrijdId is not null)
        {
            cmd.Parameters.AddWithValue("$excludeId", excludeWedstrijdId.Value);
        }

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public bool BestaatTeamConflict(int teamId, DateTime datum, string tijdslot, int? excludeWedstrijdId = null)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            excludeWedstrijdId is null
                ? """
                  SELECT COUNT(1)
                  FROM Wedstrijd
                  WHERE team_id = $teamId AND datum = $datum AND tijdslot = $tijdslot;
                  """
                : """
                  SELECT COUNT(1)
                  FROM Wedstrijd
                  WHERE team_id = $teamId
                    AND datum = $datum
                    AND tijdslot = $tijdslot
                    AND wedstrijd_id <> $excludeId;
                  """;
        cmd.Parameters.AddWithValue("$teamId", teamId);
        cmd.Parameters.AddWithValue("$datum", datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$tijdslot", tijdslot);
        if (excludeWedstrijdId is not null)
        {
            cmd.Parameters.AddWithValue("$excludeId", excludeWedstrijdId.Value);
        }

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public Wedstrijd? GetWedstrijd(int wedstrijdId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT wedstrijd_id, team_id, veld_id, datum, tijdslot, tegenstander, status
            FROM Wedstrijd
            WHERE wedstrijd_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", wedstrijdId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Wedstrijd
        {
            WedstrijdId = reader.GetInt32(0),
            TeamId = reader.GetInt32(1),
            VeldId = reader.GetInt32(2),
            Datum = DateTime.Parse(reader.GetString(3)),
            Tijdslot = reader.GetString(4),
            Tegenstander = reader.IsDBNull(5) ? null : reader.GetString(5),
            Status = reader.GetString(6),
        };
    }

    public void UpdateWedstrijd(Wedstrijd wedstrijd)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            UPDATE Wedstrijd
            SET team_id = $teamId,
                veld_id = $veldId,
                datum = $datum,
                tijdslot = $tijdslot,
                tegenstander = $tegenstander,
                status = $status
            WHERE wedstrijd_id = $id;
            """;
        cmd.Parameters.AddWithValue("$id", wedstrijd.WedstrijdId);
        cmd.Parameters.AddWithValue("$teamId", wedstrijd.TeamId);
        cmd.Parameters.AddWithValue("$veldId", wedstrijd.VeldId);
        cmd.Parameters.AddWithValue("$datum", wedstrijd.Datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$tijdslot", wedstrijd.Tijdslot);
        cmd.Parameters.AddWithValue("$tegenstander", (object?)wedstrijd.Tegenstander ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$status", wedstrijd.Status);
        cmd.ExecuteNonQuery();
    }

    public void DeleteWedstrijd(int wedstrijdId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Wedstrijd WHERE wedstrijd_id = $id;";
        cmd.Parameters.AddWithValue("$id", wedstrijdId);
        cmd.ExecuteNonQuery();
    }

    public int InsertWedstrijd(Wedstrijd wedstrijd)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            INSERT INTO Wedstrijd (team_id, veld_id, datum, tijdslot, tegenstander, status)
            VALUES ($teamId, $veldId, $datum, $tijdslot, $tegenstander, $status);
            """;
        cmd.Parameters.AddWithValue("$teamId", wedstrijd.TeamId);
        cmd.Parameters.AddWithValue("$veldId", wedstrijd.VeldId);
        cmd.Parameters.AddWithValue("$datum", wedstrijd.Datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$tijdslot", wedstrijd.Tijdslot);
        cmd.Parameters.AddWithValue("$tegenstander", (object?)wedstrijd.Tegenstander ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$status", wedstrijd.Status);
        cmd.ExecuteNonQuery();

        using var idCmd = connection.CreateCommand();
        idCmd.CommandText = "SELECT last_insert_rowid();";
        return Convert.ToInt32(idCmd.ExecuteScalar());
    }

    public List<int> GetLedenIdsVanTeam(int teamId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT lid_id
            FROM LidTeam
            WHERE team_id = $teamId;
            """;
        cmd.Parameters.AddWithValue("$teamId", teamId);

        using var reader = cmd.ExecuteReader();
        var result = new List<int>();
        while (reader.Read())
        {
            result.Add(reader.GetInt32(0));
        }

        return result;
    }

    public bool ZijnAlleLedenBeschikbaar(IEnumerable<int> lidIds, DateTime datum, string tijdslot)
    {
        var ids = lidIds.ToList();
        if (ids.Count == 0) return false;

        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        var inParams = ids.Select((_, i) => $"$id{i}").ToArray();
        cmd.CommandText =
            $"""
             SELECT COUNT(1)
             FROM Beschikbaarheid
             WHERE datum = $datum
               AND tijdslot = $tijdslot
               AND status = 'Beschikbaar'
               AND lid_id IN ({string.Join(", ", inParams)});
             """;
        cmd.Parameters.AddWithValue("$datum", datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$tijdslot", tijdslot);
        for (var i = 0; i < ids.Count; i++)
        {
            cmd.Parameters.AddWithValue(inParams[i], ids[i]);
        }

        var beschikbaarCount = Convert.ToInt32(cmd.ExecuteScalar());
        return beschikbaarCount == ids.Count;
    }
}

