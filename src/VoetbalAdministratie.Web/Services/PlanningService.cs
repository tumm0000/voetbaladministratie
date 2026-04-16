using VoetbalAdministratie.Web.Models;
using VoetbalAdministratie.Web.Repositories;

namespace VoetbalAdministratie.Web.Services;

public sealed class PlanningService
{
    private readonly PlanningRepository _planningRepository;
    private readonly LookupRepository _lookupRepository;

    public PlanningService(PlanningRepository planningRepository, LookupRepository lookupRepository)
    {
        _planningRepository = planningRepository;
        _lookupRepository = lookupRepository;
    }

    public List<PlanningItem> GetPlanning() => _planningRepository.GetPlanning();

    public Wedstrijd? GetWedstrijd(int wedstrijdId) => _planningRepository.GetWedstrijd(wedstrijdId);

    public (bool success, string message) PlanWedstrijd(Wedstrijd wedstrijd)
    {
        if (_planningRepository.BestaatVeldConflict(wedstrijd.VeldId, wedstrijd.Datum, wedstrijd.Tijdslot, excludeWedstrijdId: null))
        {
            return (false, "Dit veld is al bezet op het gekozen tijdslot.");
        }

        var lidIds = _planningRepository.GetLedenIdsVanTeam(wedstrijd.TeamId);
        if (!_planningRepository.ZijnAlleLedenBeschikbaar(lidIds, wedstrijd.Datum, wedstrijd.Tijdslot))
        {
            return (false, "Niet alle spelers van dit team zijn beschikbaar op dit tijdslot (strenge check).");
        }

        wedstrijd.Status = string.IsNullOrWhiteSpace(wedstrijd.Status) ? "Gepland" : wedstrijd.Status;
        _planningRepository.InsertWedstrijd(wedstrijd);
        return (true, "Wedstrijd is ingepland.");
    }

    public (bool success, string message) WijzigWedstrijd(Wedstrijd wedstrijd)
    {
        if (wedstrijd.WedstrijdId <= 0) return (false, "Ongeldige wedstrijd.");

        var bestaand = _planningRepository.GetWedstrijd(wedstrijd.WedstrijdId);
        if (bestaand is null) return (false, "Wedstrijd bestaat niet (meer).");

        if (_planningRepository.BestaatVeldConflict(wedstrijd.VeldId, wedstrijd.Datum, wedstrijd.Tijdslot, excludeWedstrijdId: wedstrijd.WedstrijdId))
        {
            return (false, "Dit veld is al bezet op het gekozen tijdslot.");
        }

        var lidIds = _planningRepository.GetLedenIdsVanTeam(wedstrijd.TeamId);
        if (!_planningRepository.ZijnAlleLedenBeschikbaar(lidIds, wedstrijd.Datum, wedstrijd.Tijdslot))
        {
            return (false, "Niet alle spelers van dit team zijn beschikbaar op dit tijdslot (strenge check).");
        }

        wedstrijd.Status = string.IsNullOrWhiteSpace(wedstrijd.Status) ? bestaand.Status : wedstrijd.Status;
        _planningRepository.UpdateWedstrijd(wedstrijd);
        return (true, "Wedstrijd is bijgewerkt.");
    }

    public void VerwijderWedstrijd(int wedstrijdId)
    {
        _planningRepository.DeleteWedstrijd(wedstrijdId);
    }

    public (int added, List<string> warnings) AutoPlanKomendeWeek()
    {
        var teams = _lookupRepository.GetTeams();
        var velden = _lookupRepository.GetVelden();

        if (teams.Count == 0 || velden.Count == 0)
        {
            return (0, new List<string> { "Geen teams of velden gevonden." });
        }

        var warnings = new List<string>();
        var added = 0;

        var tijdsloten = new[] { "18:00-19:30", "19:30-21:00" };
        var start = DateTime.UtcNow.Date.AddDays(1);

        foreach (var team in teams)
        {
            var plannedForTeam = false;
            for (var day = 0; day < 7 && !plannedForTeam; day++)
            {
                var datum = start.AddDays(day);
                foreach (var tijdslot in tijdsloten)
                {
                    foreach (var veld in velden)
                    {
                        if (_planningRepository.BestaatVeldConflict(veld.VeldId, datum, tijdslot, excludeWedstrijdId: null))
                        {
                            continue;
                        }

                        var lidIds = _planningRepository.GetLedenIdsVanTeam(team.TeamId);
                        if (!_planningRepository.ZijnAlleLedenBeschikbaar(lidIds, datum, tijdslot))
                        {
                            continue;
                        }

                        var wedstrijd = new Wedstrijd
                        {
                            TeamId = team.TeamId,
                            VeldId = veld.VeldId,
                            Datum = datum,
                            Tijdslot = tijdslot,
                            Tegenstander = "TBD",
                            Status = "Gepland",
                        };
                        _planningRepository.InsertWedstrijd(wedstrijd);
                        added++;
                        plannedForTeam = true;
                        break;
                    }
                    if (plannedForTeam) break;
                }
            }

            if (!plannedForTeam)
            {
                warnings.Add($"Geen geschikt tijdslot gevonden voor team '{team.Naam}' in de komende week.");
            }
        }

        return (added, warnings);
    }
}

