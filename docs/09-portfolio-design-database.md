# Portfolio: Design — database

Dit document sluit aan op de leeruitkomst rond database: je ontwerpt, bouwt en bevraagt een relationeel databasesysteem en koppelt dat aan een applicatie. Hieronder staat hoe dat in dit project is gedaan en hoe databaseontwerp zich verhoudt tot softwareontwerp.

---

## 1. Onderscheid tussen database- en softwareontwerp

Databaseontwerp gaat over welke gegevens je opslaat, in welke tabellen, met welke sleutels en welke relaties tussen tabellen. Dat staat in `docs/02-erd-conceptueel-model.md` (conceptueel), `docs/03-datamodel-sql.md` (logisch/relationeel) en `database/schema.sql` (technisch script).

Softwareontwerp gaat over hoe de applicatie dat domein en die data aanstuuurt: objecten, services, toegang tot de database en de gebruikersinterface. Dat staat in `docs/04-klassendiagram.md` en in de beschrijving van mappen en lagen in `docs/05-applicatie-toelichting.md` (modellen, repositories, services, controllers, views).

Zo hoort het relationele schema bij de “data-laag”, terwijl het klassendiagram en de MVC-structuur bij de “applicatielaag” horen. Beide ontwerpen zijn op elkaar afgestemd: dezelfde begrippen (lid, team, contributie, wedstrijd) komen in beide terug, maar op een andere manier gemodelleerd (tabellen versus klassen en code).

---

## 2. Relaties in het databaseontwerp

Het ontwerp bevat meerdere soorten relaties, zoals gevraagd bij databaseontwerp.

Een-op-veel: bijvoorbeeld een lid heeft meerdere contributieregels, een contributie kan meerdere betalingen hebben via de koppeling in de tabel voor betalingen, en een team heeft meerdere wedstrijden en trainingen gekoppeld aan hetzelfde team-id.

Veel-op-veel: een lid kan bij meerdere teams horen en een team heeft meerdere leden. Dat is opgelost met een koppeltabel (lid–team), zodat de database normalisatievol en uitbreidbaar blijft.

Deze keuzes staan toegelicht in `docs/03-datamodel-sql.md` en in het conceptuele model in `docs/02-erd-conceptueel-model.md`.

---

## 3. Bouwen en koppelen aan de applicatie

De database wordt technisch aangemaakt en gevuld in het kader van het prototype; de applicatie gebruikt ADO.NET om SQL uit te voeren. Details over opstarten, dummy-data en structuur staan in `docs/05-applicatie-toelichting.md`. Daarmee is het relationele model daadwerkelijk geïntegreerd met de webapplicatie, niet alleen op papier.

---

## 4. Bevragen: CRUD en performance

CRUD (aanmaken, lezen, wijzigen, verwijderen waar van toepassing) gebeurt via de repository-laag: queries en commando’s op de tabellen voor leden, teams, contributies, betalingen, wedstrijden enzovoort. Dat sluit aan bij het ontwerp van tabellen en sleutels, zodat elke bewerking een duidelijke plek in het schema heeft.

Voor performance is in dit prototype bewust gekozen voor eenvoud: SQLite als lokaal bestand, gerichte queries en geen onnodig zware joins voor een kleine dataset. Voor een echte club met veel data zouden indexen op veelgebruikte kolommen (bijvoorbeeld datum, team-id, veld-id) en verder onderzoek naar trage queries een logische volgende stap zijn; dat past bij het idee dat je bij bevragen rekening houdt met hoe snel en schaalbaar het blijft.

---

## 5. Korte conclusie

Database-design (ERD, tabellen, relaties 1-op-veel en veel-op-veel) en software-design (UML, lagen, UI) zijn bewust van elkaar gescheiden maar op elkaar afgestemd. Het relationele model is gebouwd, bevraagd via CRUD in de applicatie en uitgelegd in de documentatie, wat aansluit op de database-eisen binnen het portfolio-onderdeel Design.
