using Microsoft.AspNetCore.Mvc;
using VoetbalAdministratie.Web.Models;
using VoetbalAdministratie.Web.Repositories;
using VoetbalAdministratie.Web.Services;

namespace VoetbalAdministratie.Web.Controllers;

public sealed class PlanningController : Controller
{
    private readonly PlanningService _planningService;
    private readonly LookupRepository _lookupRepository;

    public PlanningController(PlanningService planningService, LookupRepository lookupRepository)
    {
        _planningService = planningService;
        _lookupRepository = lookupRepository;
    }

    public IActionResult Index()
    {
        var planning = _planningService.GetPlanning();
        return View(planning);
    }

    [HttpGet]
    public IActionResult Nieuw()
    {
        ViewBag.Teams = _lookupRepository.GetTeams();
        ViewBag.Velden = _lookupRepository.GetVelden();
        return View(new Wedstrijd { Datum = DateTime.UtcNow.Date.AddDays(1), Tijdslot = "", Status = "Gepland" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Nieuw(Wedstrijd wedstrijd)
    {
        ViewBag.Teams = _lookupRepository.GetTeams();
        ViewBag.Velden = _lookupRepository.GetVelden();

        if (wedstrijd.TeamId <= 0) ModelState.AddModelError(nameof(wedstrijd.TeamId), "Team is verplicht.");
        if (wedstrijd.VeldId <= 0) ModelState.AddModelError(nameof(wedstrijd.VeldId), "Veld is verplicht.");
        if (string.IsNullOrWhiteSpace(wedstrijd.Tijdslot)) ModelState.AddModelError(nameof(wedstrijd.Tijdslot), "Tijdslot is verplicht.");

        if (!ModelState.IsValid) return View(wedstrijd);

        var (success, message) = _planningService.PlanWedstrijd(wedstrijd);
        TempData[success ? "Message" : "Error"] = message;
        return success ? RedirectToAction(nameof(Index)) : View(wedstrijd);
    }

    [HttpGet]
    public IActionResult Wijzig(int id)
    {
        var wedstrijd = _planningService.GetWedstrijd(id);
        if (wedstrijd is null) return NotFound();

        ViewBag.Teams = _lookupRepository.GetTeams();
        ViewBag.Velden = _lookupRepository.GetVelden();
        return View(wedstrijd);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Wijzig(Wedstrijd wedstrijd)
    {
        ViewBag.Teams = _lookupRepository.GetTeams();
        ViewBag.Velden = _lookupRepository.GetVelden();

        if (wedstrijd.WedstrijdId <= 0) return BadRequest();
        if (wedstrijd.TeamId <= 0) ModelState.AddModelError(nameof(wedstrijd.TeamId), "Team is verplicht.");
        if (wedstrijd.VeldId <= 0) ModelState.AddModelError(nameof(wedstrijd.VeldId), "Veld is verplicht.");
        if (string.IsNullOrWhiteSpace(wedstrijd.Tijdslot)) ModelState.AddModelError(nameof(wedstrijd.Tijdslot), "Tijdslot is verplicht.");

        if (!ModelState.IsValid) return View(wedstrijd);

        var (success, message) = _planningService.WijzigWedstrijd(wedstrijd);
        TempData[success ? "Message" : "Error"] = message;
        return success ? RedirectToAction(nameof(Index)) : View(wedstrijd);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Verwijder(int wedstrijdId)
    {
        if (wedstrijdId <= 0) return BadRequest();
        _planningService.VerwijderWedstrijd(wedstrijdId);
        TempData["Message"] = "Wedstrijd is verwijderd.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AutoPlan()
    {
        var (added, warnings) = _planningService.AutoPlanKomendeWeek();
        TempData["Message"] = $"Auto-plan afgerond: {added} wedstrijd(en) toegevoegd.";
        if (warnings.Count > 0) TempData["Warnings"] = warnings;
        return RedirectToAction(nameof(Index));
    }
}

