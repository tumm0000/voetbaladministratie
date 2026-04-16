# Stap 5: Werkende applicatie (Prototype)

In deze stap is een werkend prototype gerealiseerd als **ASP.NET Core MVC webapplicatie** in **C#**, met database-toegang via **ADO.NET** (zonder ORM). De applicatie ondersteunt de kernfunctionaliteiten uit de analyse: ledenbeheer, contributie/betalingen en wedstrijdplanning met beschikbaarheid.

---

## 1. Techniek en structuur

- **Framework:** ASP.NET Core MVC (Razor Views), .NET 7
- **Database:** SQLite (lokaal bestand)
- **Data access:** ADO.NET (`Microsoft.Data.Sqlite`)
- **Architectuur (globaal):**
  - `Models/`: domeinobjecten (o.a. `Lid`, `Contributie`, `Wedstrijd`)
  - `Repositories/`: SQL/ADO.NET queries en commands
  - `Services/`: use-case logica (plannen, betalingen, ledenbeheer)
  - `Controllers/` + `Views/`: webinterface

Bij het opstarten wordt automatisch de database aangemaakt en gevuld met **dummy-data** (leden, teams, velden, contributies, beschikbaarheid en een eerste wedstrijd).

---

## 2. Functionaliteit (prototype)

### Ledenbeheer
- Leden inzien
- Nieuw lid toevoegen
- Lidgegevens wijzigen

### Contributie en betalingen
- Overzicht van **alle contributieregels** (status `Open` en `Betaald`), met open items bovenaan (op basis van vervaldatum)
- Betaling registreren (zet contributie op `Betaald`)

### Wedstrijdplanning
- Planningoverzicht (alle ingeplande wedstrijden)
- Handmatig een wedstrijd inplannen met conflictcontrole op veld+tijdslot
- **Auto-plan**: probeert voor ieder team binnen 7 dagen één wedstrijd te plannen
- Beschikbaarheid: **strenge check** — alle spelers van het team moeten “Beschikbaar” zijn op datum+tijdslot

---

## 3. Installatie en draaien

1. Ga naar de repo-root.
2. Start de webapp:

```bash
dotnet run --project src/VoetbalAdministratie.Web
```

3. Open de URL die in de terminal verschijnt.

De SQLite database wordt aangemaakt op: `src/VoetbalAdministratie.Web/App_Data/voetbaladministratie.db`.

---

## 4. Testresultaten (kort)

- **Leden toevoegen/wijzigen:** succesvol; wijzigingen zijn direct zichtbaar in de ledenlijst.
- **Betaling registreren:** succesvol; item wordt gemarkeerd als `Betaald` en verschuift automatisch onderaan.
- **Wedstrijd plannen:** veldconflict wordt geblokkeerd; bij onvoldoende beschikbaarheid wordt plannen afgekeurd met melding.
- **Auto-plan:** voegt wedstrijden toe waar mogelijk en geeft waarschuwingen als er geen geschikt tijdslot gevonden wordt.

