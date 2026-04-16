using VoetbalAdministratie.Web.Models;

namespace VoetbalAdministratie.Web.Services;

public static class TeamLeeftijdPolicy
{
    private enum TeamLeeftijdGroep
    {
        Onbekend,
        Senioren,
        Jeugd,
    }

    public static string? ValideerKoppeling(Team team, DateTime geboortedatum, DateTime referentieDatum)
    {
        var leeftijd = BerekenLeeftijd(geboortedatum, referentieDatum);
        if (leeftijd < 0) return "Geboortedatum kan niet in de toekomst liggen.";

        var groep = Classificeer(team.Categorie);
        return groep switch
        {
            TeamLeeftijdGroep.Senioren when leeftijd < 18 =>
                "Dit team valt onder senioren: een lid moet minimaal 18 jaar zijn op de koppeldatum.",
            TeamLeeftijdGroep.Jeugd when leeftijd >= 18 =>
                "Dit team valt onder jeugd: een lid moet jonger dan 18 jaar zijn op de koppeldatum.",
            _ => null,
        };
    }

    private static TeamLeeftijdGroep Classificeer(string categorie)
    {
        var c = categorie.Trim();

        // Voorbeelden in seed: "Senioren", "Jeugd"
        if (c.Contains("senior", StringComparison.OrdinalIgnoreCase)) return TeamLeeftijdGroep.Senioren;
        if (c.Contains("jeugd", StringComparison.OrdinalIgnoreCase)) return TeamLeeftijdGroep.Jeugd;

        // Heuristiek voor jeugdteams zoals "JO17-1"
        if (c.StartsWith("JO", StringComparison.OrdinalIgnoreCase)) return TeamLeeftijdGroep.Jeugd;

        return TeamLeeftijdGroep.Onbekend;
    }

    private static int BerekenLeeftijd(DateTime geboortedatum, DateTime referentieDatum)
    {
        var refDate = referentieDatum.Date;
        var birth = geboortedatum.Date;

        var age = refDate.Year - birth.Year;
        if (refDate.Month < birth.Month || (refDate.Month == birth.Month && refDate.Day < birth.Day))
        {
            age--;
        }

        return age;
    }
}
