namespace VoetbalAdministratie.Web.Models;

public sealed class Contributie
{
    public int ContributieId { get; set; }
    public int LidId { get; set; }
    public decimal Bedrag { get; set; }
    public DateTime Vervaldatum { get; set; }
    public string Status { get; set; } = "";
}

