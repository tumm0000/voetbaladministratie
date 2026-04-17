using System.ComponentModel.DataAnnotations;

namespace VoetbalAdministratie.Web.ViewModels;

public sealed class LidTeamKoppelViewModel
{
    public int LidId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Team is verplicht.")]
    public int TeamId { get; set; }

    [Required(ErrorMessage = "Vanaf datum is verplicht.")]
    [DataType(DataType.Date)]
    public DateTime VanafDatum { get; set; } = DateTime.UtcNow.Date;
}
