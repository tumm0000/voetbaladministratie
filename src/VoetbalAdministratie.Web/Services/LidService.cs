using VoetbalAdministratie.Web.Models;
using VoetbalAdministratie.Web.Repositories;

namespace VoetbalAdministratie.Web.Services;

public sealed class LidService
{
    private readonly LidRepository _lidRepository;
    private readonly LookupRepository _lookupRepository;
    private readonly LidTeamRepository _lidTeamRepository;

    public LidService(LidRepository lidRepository, LookupRepository lookupRepository, LidTeamRepository lidTeamRepository)
    {
        _lidRepository = lidRepository;
        _lookupRepository = lookupRepository;
        _lidTeamRepository = lidTeamRepository;
    }

    public List<Lid> GetLeden() => _lidRepository.GetAll();

    public Lid? GetLid(int id) => _lidRepository.GetById(id);

    public List<Team> GetTeams() => _lookupRepository.GetTeams();

    public List<LidTeamLidmaatschap> GetLidmaatschappen(int lidId) => _lidTeamRepository.GetLidmaatschappenVoorLid(lidId);

    public int Registreer(Lid lid) => _lidRepository.Insert(lid);

    public void Wijzig(Lid lid) => _lidRepository.Update(lid);

    public (bool success, string message) KoppelLidAanTeam(int lidId, int teamId, DateTime vanafDatum)
    {
        if (lidId <= 0 || teamId <= 0) return (false, "Ongeldige invoer.");

        var lid = _lidRepository.GetById(lidId);
        if (lid is null) return (false, "Lid bestaat niet.");
        if (lid.Geboortedatum is null) return (false, "Geboortedatum ontbreekt. Vul dit eerst in bij het lid.");

        var team = _lookupRepository.GetTeamById(teamId);
        if (team is null) return (false, "Team bestaat niet.");

        if (_lidTeamRepository.BestaatKoppeling(lidId, teamId))
        {
            return (false, "Dit lid is al gekoppeld aan dit team.");
        }

        var fout = TeamLeeftijdPolicy.ValideerKoppeling(team, lid.Geboortedatum.Value, vanafDatum);
        if (fout is not null) return (false, fout);

        _lidTeamRepository.Insert(lidId, teamId, vanafDatum);
        return (true, "Lid is gekoppeld aan het team.");
    }

    public (bool success, string message) VerwijderLidmaatschap(int lidTeamId, int lidId)
    {
        if (lidTeamId <= 0 || lidId <= 0) return (false, "Ongeldige invoer.");

        var lid = _lidRepository.GetById(lidId);
        if (lid is null) return (false, "Lid bestaat niet.");

        var ok = _lidTeamRepository.VerwijderLidmaatschap(lidTeamId, lidId);
        return ok
            ? (true, "Teamkoppeling is verwijderd.")
            : (false, "Teamkoppeling bestaat niet (meer).");
    }
}

