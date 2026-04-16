using Microsoft.AspNetCore.Mvc;
using VoetbalAdministratie.Web.Models;
using VoetbalAdministratie.Web.Services;
using VoetbalAdministratie.Web.ViewModels;

namespace VoetbalAdministratie.Web.Controllers;

public sealed class LedenController : Controller
{
    private readonly LidService _lidService;

    public LedenController(LidService lidService)
    {
        _lidService = lidService;
    }

    public IActionResult Index()
    {
        var leden = _lidService.GetLeden();
        return View(leden);
    }

    [HttpGet]
    public IActionResult Nieuw()
    {
        return View(new Lid
        {
            Status = "Actief",
            LidmaatschapType = "Spelend",
            Geboortedatum = new DateTime(2008, 1, 1),
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Nieuw(Lid lid)
    {
        if (string.IsNullOrWhiteSpace(lid.Voornaam)) ModelState.AddModelError(nameof(lid.Voornaam), "Voornaam is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Achternaam)) ModelState.AddModelError(nameof(lid.Achternaam), "Achternaam is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Email)) ModelState.AddModelError(nameof(lid.Email), "E-mail is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.LidmaatschapType)) ModelState.AddModelError(nameof(lid.LidmaatschapType), "Type is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Status)) ModelState.AddModelError(nameof(lid.Status), "Status is verplicht.");
        if (lid.Geboortedatum is null) ModelState.AddModelError(nameof(lid.Geboortedatum), "Geboortedatum is verplicht.");
        else if (lid.Geboortedatum.Value.Date > DateTime.UtcNow.Date)
            ModelState.AddModelError(nameof(lid.Geboortedatum), "Geboortedatum kan niet in de toekomst liggen.");

        if (!ModelState.IsValid) return View(lid);

        _lidService.Registreer(lid);
        TempData["Message"] = "Lid is toegevoegd.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Wijzig(int id)
    {
        var lid = _lidService.GetLid(id);
        if (lid is null) return NotFound();
        return View(lid);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Wijzig(Lid lid)
    {
        if (lid.LidId <= 0) return BadRequest();

        if (string.IsNullOrWhiteSpace(lid.Voornaam)) ModelState.AddModelError(nameof(lid.Voornaam), "Voornaam is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Achternaam)) ModelState.AddModelError(nameof(lid.Achternaam), "Achternaam is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Email)) ModelState.AddModelError(nameof(lid.Email), "E-mail is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.LidmaatschapType)) ModelState.AddModelError(nameof(lid.LidmaatschapType), "Type is verplicht.");
        if (string.IsNullOrWhiteSpace(lid.Status)) ModelState.AddModelError(nameof(lid.Status), "Status is verplicht.");
        if (lid.Geboortedatum is null) ModelState.AddModelError(nameof(lid.Geboortedatum), "Geboortedatum is verplicht.");
        else if (lid.Geboortedatum.Value.Date > DateTime.UtcNow.Date)
            ModelState.AddModelError(nameof(lid.Geboortedatum), "Geboortedatum kan niet in de toekomst liggen.");

        if (!ModelState.IsValid) return View(lid);

        _lidService.Wijzig(lid);
        TempData["Message"] = "Lid is bijgewerkt.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Team(int id)
    {
        var lid = _lidService.GetLid(id);
        if (lid is null) return NotFound();

        ViewBag.LidNaam = lid.VolledigeNaam;
        ViewBag.Teams = _lidService.GetTeams();
        ViewBag.Lidmaatschappen = _lidService.GetLidmaatschappen(id);

        return View(new LidTeamKoppelViewModel
        {
            LidId = id,
            VanafDatum = DateTime.UtcNow.Date,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Team(LidTeamKoppelViewModel vm)
    {
        ViewBag.LidNaam = _lidService.GetLid(vm.LidId)?.VolledigeNaam ?? "Lid";
        ViewBag.Teams = _lidService.GetTeams();
        ViewBag.Lidmaatschappen = _lidService.GetLidmaatschappen(vm.LidId);

        if (vm.LidId <= 0) return BadRequest();
        if (vm.TeamId <= 0) ModelState.AddModelError(nameof(vm.TeamId), "Team is verplicht.");

        if (!ModelState.IsValid) return View(vm);

        var (success, message) = _lidService.KoppelLidAanTeam(vm.LidId, vm.TeamId, vm.VanafDatum);
        TempData[success ? "Message" : "Error"] = message;
        return success ? RedirectToAction(nameof(Team), new { id = vm.LidId }) : View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult VerwijderLidTeam(int lidTeamId, int lidId)
    {
        if (lidTeamId <= 0 || lidId <= 0) return BadRequest();

        var (success, message) = _lidService.VerwijderLidmaatschap(lidTeamId, lidId);
        TempData[success ? "Message" : "Error"] = message;
        return RedirectToAction(nameof(Team), new { id = lidId });
    }
}

