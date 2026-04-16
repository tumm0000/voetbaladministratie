using VoetbalAdministratie.Web.Repositories;

namespace VoetbalAdministratie.Web.Services;

public sealed class BetalingService
{
    private readonly BetalingRepository _betalingRepository;

    public BetalingService(BetalingRepository betalingRepository)
    {
        _betalingRepository = betalingRepository;
    }

    public List<BetalingItem> GetBetalingen() => _betalingRepository.GetBetalingen();

    public (int contributieId, int lidId, decimal bedrag, DateTime vervaldatum, string status)? GetContributie(int contributieId)
        => _betalingRepository.GetContributie(contributieId);

    public int MaakContributieAan(int lidId, decimal bedrag, DateTime vervaldatum, string status)
        => _betalingRepository.InsertContributie(lidId, bedrag, vervaldatum, status);

    public void WijzigContributie(int contributieId, int lidId, decimal bedrag, DateTime vervaldatum, string status)
        => _betalingRepository.UpdateContributie(contributieId, lidId, bedrag, vervaldatum, status);

    public void VerwijderContributie(int contributieId)
        => _betalingRepository.DeleteContributie(contributieId);

    public void RegistreerBetaling(int contributieId, decimal bedrag, DateTime betaaldatum, string betaalmethode)
    {
        _betalingRepository.InsertBetaling(contributieId, bedrag, betaaldatum, betaalmethode);
    }
}

