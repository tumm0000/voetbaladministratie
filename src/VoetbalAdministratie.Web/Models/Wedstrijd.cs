namespace VoetbalAdministratie.Web.Models;

public sealed class Wedstrijd
{
    public int WedstrijdId { get; set; }
    public int TeamId { get; set; }
    public int VeldId { get; set; }
    public DateTime Datum { get; set; }
    public string Tijdslot { get; set; } = "";
    public string? Tegenstander { get; set; }
    public string Status { get; set; } = "";
}

