using System.ComponentModel.DataAnnotations;

namespace VoetbalAdministratie.Web.ViewModels;

public sealed class ContributieEditViewModel
{
    public int ContributieId { get; set; }

    [Required]
    public int LidId { get; set; }

    [Required]
    [Range(0.01, 999999)]
    public decimal Bedrag { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Vervaldatum { get; set; }

    [Required]
    public string Status { get; set; } = "Open";
}

