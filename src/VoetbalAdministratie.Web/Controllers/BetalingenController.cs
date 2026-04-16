using Microsoft.AspNetCore.Mvc;
using VoetbalAdministratie.Web.Services;
using VoetbalAdministratie.Web.ViewModels;

namespace VoetbalAdministratie.Web.Controllers;

public sealed class BetalingenController : Controller
{
    private readonly BetalingService _betalingService;
    private readonly LidService _lidService;

    public BetalingenController(BetalingService betalingService, LidService lidService)
    {
        _betalingService = betalingService;
        _lidService = lidService;
    }

    public IActionResult Index()
    {
        var items = _betalingService.GetBetalingen();
        return View(items);
    }

    [HttpGet]
    public IActionResult Nieuw()
    {
        ViewBag.Leden = _lidService.GetLeden();
        return View(new ContributieEditViewModel
        {
            Vervaldatum = DateTime.UtcNow.Date.AddDays(14),
            Status = "Open",
            Bedrag = 25.00m
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Nieuw(ContributieEditViewModel vm)
    {
        ViewBag.Leden = _lidService.GetLeden();
        if (!ModelState.IsValid) return View(vm);

        _betalingService.MaakContributieAan(vm.LidId, vm.Bedrag, vm.Vervaldatum, vm.Status);
        TempData["Message"] = "Contributieregel is aangemaakt.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Wijzig(int id)
    {
        var c = _betalingService.GetContributie(id);
        if (c is null) return NotFound();

        ViewBag.Leden = _lidService.GetLeden();
        return View(new ContributieEditViewModel
        {
            ContributieId = c.Value.contributieId,
            LidId = c.Value.lidId,
            Bedrag = c.Value.bedrag,
            Vervaldatum = c.Value.vervaldatum,
            Status = c.Value.status
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Wijzig(ContributieEditViewModel vm)
    {
        ViewBag.Leden = _lidService.GetLeden();
        if (vm.ContributieId <= 0) return BadRequest();
        if (!ModelState.IsValid) return View(vm);

        _betalingService.WijzigContributie(vm.ContributieId, vm.LidId, vm.Bedrag, vm.Vervaldatum, vm.Status);
        TempData["Message"] = "Contributieregel is bijgewerkt.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Verwijder(int contributieId)
    {
        if (contributieId <= 0) return BadRequest();
        _betalingService.VerwijderContributie(contributieId);
        TempData["Message"] = "Contributieregel is verwijderd.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registreer(int contributieId, decimal bedrag, string betaalmethode)
    {
        if (contributieId <= 0) return BadRequest();
        if (bedrag <= 0) return BadRequest();
        if (string.IsNullOrWhiteSpace(betaalmethode)) betaalmethode = "Onbekend";

        _betalingService.RegistreerBetaling(contributieId, bedrag, DateTime.UtcNow.Date, betaalmethode);
        TempData["Message"] = "Betaling is geregistreerd en contributie is op 'Betaald' gezet.";
        return RedirectToAction(nameof(Index));
    }
}

