namespace VoetbalAdministratie.Web.Models;

public sealed class Veld
{
    public int VeldId { get; set; }
    public string Naam { get; set; } = "";
    public string? Locatie { get; set; }
}

