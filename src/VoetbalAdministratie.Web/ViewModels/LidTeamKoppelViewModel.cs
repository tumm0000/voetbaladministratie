using System.ComponentModel.DataAnnotations;

namespace VoetbalAdministratie.Web.ViewModels;

public sealed class LidTeamKoppelViewModel
{
    public int LidId { get; set; }

    [Required]
    public int TeamId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime VanafDatum { get; set; } = DateTime.UtcNow.Date;
}
