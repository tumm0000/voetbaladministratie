namespace VoetbalAdministratie.Web.Models;

public sealed class Betaling
{
    public int BetalingId { get; set; }
    public int ContributieId { get; set; }
    public decimal Bedrag { get; set; }
    public DateTime Betaaldatum { get; set; }
    public string? Betaalmethode { get; set; }
}

