namespace VoetbalAdministratie.Web.Models;

public sealed class Team
{
    public int TeamId { get; set; }
    public string Naam { get; set; } = "";
    public string Categorie { get; set; } = "";
}

