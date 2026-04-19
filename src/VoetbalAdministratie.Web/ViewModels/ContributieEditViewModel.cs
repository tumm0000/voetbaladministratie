using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VoetbalAdministratie.Web.ViewModels;

public sealed class ContributieEditViewModel : IValidatableObject
{
    public int ContributieId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecteer een lid.")]
    public int LidId { get; set; }

    [Required(ErrorMessage = "Bedrag is verplicht.")]
    [RegularExpression(
        @"^((\d+([.,]\d{1,2})?)|([.,]\d{1,2}))$",
        ErrorMessage = "Voer een geldig geldbedrag in met maximaal 2 decimalen (bijv. 10,50 of 10.50).")]
    public string Bedrag { get; set; } = "";

    [Required(ErrorMessage = "Vervaldatum is verplicht.")]
    [DataType(DataType.Date)]
    public DateTime Vervaldatum { get; set; }

    [Required(ErrorMessage = "Status is verplicht.")]
    public string Status { get; set; } = "Open";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!TryGetBedrag(out var bedrag))
        {
            yield return new ValidationResult(
                "Bedrag is ongeldig.",
                new[] { nameof(Bedrag) });
            yield break;
        }

        if (bedrag <= 0m)
        {
            yield return new ValidationResult(
                "Bedrag moet groter zijn dan 0.",
                new[] { nameof(Bedrag) });
        }
    }

    public bool TryGetBedrag(out decimal bedrag)
    {
        var normalized = (Bedrag ?? string.Empty).Trim().Replace(',', '.');
        if (normalized.StartsWith('.'))
        {
            normalized = "0" + normalized;
        }

        return decimal.TryParse(
            normalized,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out bedrag);
    }
}

