using Microsoft.Data.Sqlite;
using VoetbalAdministratie.Web.Data;

namespace VoetbalAdministratie.Web.Repositories;

public sealed record LidTeamLidmaatschap(
    int LidTeamId,
    int LidId,
    int TeamId,
    string TeamNaam,
    string TeamCategorie,
    DateTime VanafDatum
);

public sealed class LidTeamRepository
{
    private readonly Db _db;

    public LidTeamRepository(Db db)
    {
        _db = db;
    }

    public bool BestaatKoppeling(int lidId, int teamId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT COUNT(1)
            FROM LidTeam
            WHERE lid_id = $lidId AND team_id = $teamId;
            """;
        cmd.Parameters.AddWithValue("$lidId", lidId);
        cmd.Parameters.AddWithValue("$teamId", teamId);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public void Insert(int lidId, int teamId, DateTime vanafDatum)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            INSERT INTO LidTeam (lid_id, team_id, vanaf_datum)
            VALUES ($lidId, $teamId, $vanaf);
            """;
        cmd.Parameters.AddWithValue("$lidId", lidId);
        cmd.Parameters.AddWithValue("$teamId", teamId);
        cmd.Parameters.AddWithValue("$vanaf", vanafDatum.ToString("yyyy-MM-dd"));
        cmd.ExecuteNonQuery();
    }

    public List<LidTeamLidmaatschap> GetLidmaatschappenVoorLid(int lidId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            SELECT
                lt.lid_team_id,
                lt.lid_id,
                lt.team_id,
                t.naam,
                t.categorie,
                lt.vanaf_datum
            FROM LidTeam lt
            JOIN Team t ON t.team_id = lt.team_id
            WHERE lt.lid_id = $lidId
            ORDER BY t.naam ASC;
            """;
        cmd.Parameters.AddWithValue("$lidId", lidId);

        using var reader = cmd.ExecuteReader();
        var result = new List<LidTeamLidmaatschap>();
        while (reader.Read())
        {
            result.Add(new LidTeamLidmaatschap(
                LidTeamId: reader.GetInt32(0),
                LidId: reader.GetInt32(1),
                TeamId: reader.GetInt32(2),
                TeamNaam: reader.GetString(3),
                TeamCategorie: reader.GetString(4),
                VanafDatum: DateTime.Parse(reader.GetString(5))
            ));
        }

        return result;
    }

    public bool VerwijderLidmaatschap(int lidTeamId, int lidId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            """
            DELETE FROM LidTeam
            WHERE lid_team_id = $lidTeamId AND lid_id = $lidId;
            """;
        cmd.Parameters.AddWithValue("$lidTeamId", lidTeamId);
        cmd.Parameters.AddWithValue("$lidId", lidId);
        return cmd.ExecuteNonQuery() > 0;
    }
}
