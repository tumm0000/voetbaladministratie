namespace VoetbalAdministratie.Web.Models;

public sealed class Lid
{
    public int LidId { get; set; }
    public string Voornaam { get; set; } = "";
    public string Achternaam { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Telefoon { get; set; }
    public DateTime? Geboortedatum { get; set; }
    public string LidmaatschapType { get; set; } = "";
    public string Status { get; set; } = "";

    public string VolledigeNaam => $"{Voornaam} {Achternaam}";
}

