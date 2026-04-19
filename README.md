# Slimme Voetbalclub Administratie

Software voor ledenbeheer en wedstrijdplanning van een lokale voetbalclub. Dit project is een individuele opdracht: ontwerp en werkend prototype voor een administratiesysteem dat leden, contributies en wedstrijden beheert.

**Repository:** [https://github.com/tumm0000/voetbaladministratie](https://github.com/tumm0000/voetbaladministratie)

## Wat doet dit project?

- **Ledenbeheer** — Registratie en beheer van leden en hun gegevens (naam, contact, lidmaatschap)
- **Contributie** — Inning van contributies en automatische herinneringen bij achterstallige betalingen
- **Wedstrijdplanning** — Planning van wedstrijden met inachtneming van beschikbaarheid van spelers en velden
- **Database** — Teamindelingen, trainingen, wedstrijden en betalingen in een relationele database
- **Interface** — Eenvoudige webinterface voor bestuur en trainers

## Mapstructuur

| Map | Inhoud |
|-----|--------|
| `docs/` | Documentatie: plan van aanpak, analyse, ontwerp (ERD, datamodel, UML), applicatie, portfolio-reflecties |
| `docs/diagrams/` | Diagrammen (contextdiagram, use case, enz.) |
| `plan/` | Uitvoeringsplan voor ontwikkeling en Git-workflow |
| `database/` | SQL-schema en scripts voor de database |
| `src/` | Broncode van de C#-webapplicatie |

## Documentatie (opleveringen)

| Bestand | Beschrijving |
|---------|--------------|
| `docs/00-plan-van-aanpak.md` | Plan van aanpak, scope, fasering, Portflow-criteria |
| `docs/01-analyse.md` | Analyse: functionaliteiten, use cases, contextdiagram |
| `docs/02-erd-conceptueel-model.md` | ERD en conceptueel model |
| `docs/03-datamodel-sql.md` | Datamodel en toelichting bij het SQL-schema |
| `docs/04-klassendiagram.md` | UML-klassendiagram |
| `docs/05-applicatie-toelichting.md` | Werking, instructies en testresultaten van de applicatie |
| `docs/06-zelfreflectie-personal-leadership.md` | Zelfreflectie: Personal leadership (Portflow) |
| `docs/07-manage-and-control-reflectie.md` | Reflectie: Manage and control; uitleg codestructuur (MVC, services, repositories) |
| `docs/08-portfolio-design.md` | Portfolio: Design (gebruikersspecificaties, diagrammen, validatie) |
| `docs/09-portfolio-design-database.md` | Portfolio: Design — database (ontwerp, relaties, CRUD) |
| `docs/10-portfolio-realisatie.md` | Portfolio: Realisatie (implementatie, validatie, kwaliteit, OO) |
| `docs/11-onderzoek-tools-methoden-professional-standard.md` | Onderzoek: tools, methoden, Razor/MVC, UML, plan van aanpak, PS-1–PS-4 |

Het SQL-schema staat in `database/schema.sql`.

## Lokaal draaien

De applicatie is een ASP.NET Core MVC webapp (C#, .NET 7) met SQLite.

1. Zorg dat je de .NET SDK geïnstalleerd hebt (minimaal .NET 7).
2. Start de webapp vanuit de repo-root:

```bash
dotnet run --project src/VoetbalAdministratie.Web
```

Bij de eerste start wordt automatisch een SQLite database aangemaakt en gevuld met dummy-data.

## Techniek

- **Taal:** C#
- **Applicatie:** ASP.NET Core (webapplicatie)
- **Database:** Relationeel; toegang via ADO.NET (geen Entity Framework)
- **Versiebeheer:** Git
