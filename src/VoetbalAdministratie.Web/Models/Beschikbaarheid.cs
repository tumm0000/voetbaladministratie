namespace VoetbalAdministratie.Web.Models;

public sealed class Beschikbaarheid
{
    public int BeschikbaarheidId { get; set; }
    public int LidId { get; set; }
    public DateTime Datum { get; set; }
    public string Tijdslot { get; set; } = "";
    public string Status { get; set; } = "";
}

