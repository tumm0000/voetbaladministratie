using VoetbalAdministratie.Web.Data;
using VoetbalAdministratie.Web.Models;

namespace VoetbalAdministratie.Web.Repositories;

public sealed class LookupRepository
{
    private readonly Db _db;

    public LookupRepository(Db db)
    {
        _db = db;
    }

    public List<Team> GetTeams()
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT team_id, naam, categorie FROM Team ORDER BY naam;";
        using var reader = cmd.ExecuteReader();

        var result = new List<Team>();
        while (reader.Read())
        {
            result.Add(new Team
            {
                TeamId = reader.GetInt32(0),
                Naam = reader.GetString(1),
                Categorie = reader.GetString(2),
            });
        }

        return result;
    }

    public Team? GetTeamById(int teamId)
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT team_id, naam, categorie FROM Team WHERE team_id = $id;";
        cmd.Parameters.AddWithValue("$id", teamId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Team
        {
            TeamId = reader.GetInt32(0),
            Naam = reader.GetString(1),
            Categorie = reader.GetString(2),
        };
    }

    public List<Veld> GetVelden()
    {
        using var connection = _db.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT veld_id, naam, locatie FROM Veld ORDER BY naam;";
        using var reader = cmd.ExecuteReader();

        var result = new List<Veld>();
        while (reader.Read())
        {
            result.Add(new Veld
            {
                VeldId = reader.GetInt32(0),
                Naam = reader.GetString(1),
                Locatie = reader.IsDBNull(2) ? null : reader.GetString(2),
            });
        }

        return result;
    }
}

