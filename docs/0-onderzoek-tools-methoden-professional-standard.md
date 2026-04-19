# Onderzoek: tools, methoden en professionele praktijk

Dit document beschrijft welke tools en methoden in dit project zijn gebruikt en waarom die passen bij de opdracht en het portfolio. Het koppelt daarbij aan de professionele standaarden PS-1 tot en met PS-4. Dit is een individuele opdracht: er is geen vaste projectgroep en geen echte club als opdrachtgever aan tafel. Daarom ligt de nadruk hier op verkennend onderzoek, keuzes onderbouwen, projectorganisatie en rapportage. Waar “teams” en “stakeholders” in de criteria voorkomen, wordt dat vertaald naar samenwerking en afstemming met docent en coach en naar de beoordelaar als lezer van repository en Portflow.

---

## 1. Professionele standaarden (PS-1 tot PS-4) in dit kader

| Code | Kern van het criterium | Hoe dit in dit project terugkomt |
|------|-------------------------|----------------------------------|
| PS-1 | Professionele praktijk: organisatie, communicatie met belanghebbenden, verkennend onderzoek, rapportage | Plan van aanpak en gefaseerde `docs/`, Git, README, inlevering Portflow; tussentijdse afstemming met coach en docent |
| PS-2 | Doelen zetten, belanghebbenden betrekken, onderzoek, advies, besluiten, rapporteren; ethisch en duurzaam | Doelen en scope in `docs/00-plan-van-aanpak.md`; keuzes vastgelegd in documentatie; bewuste scopegrenzen (geen echte mails, geen productiedata) |
| PS-3 | Regie bij ICT-vraagstukken, onderzoeksmethoden, advies in complexere situaties, keuzes onderbouwen (ethiek, intercultureel, duurzaam) | Bijvoorbeeld afweging ORM versus ADO.NET, veiligheid (SQL-injectie), en ontwerpvraagstukken die tijdens feedback naar boven kwamen |
| PS-4 | Professionele praktijk in teams, inclusief ethiek, interculturaliteit, duurzaamheid | In dit project vooral individueel uitgevoerd; “team” is hier beperkt tot begeleiding en peer-context in de opleiding; ethiek en duurzaamheid kort onderbouwd in sectie 7 |

---

## 2. Waarom dit plan van aanpak

Het plan van aanpak in `docs/00-plan-van-aanpak.md` sluit direct aan op de vijf opdrachtstappen van Fontys: analyse, conceptueel model, databaseontwerp, klassendiagram, werkende applicatie. Dat is niet willekeurig gekozen maar volgt uit de opdracht en uit Portflow: elke stap heeft een duidelijke oplevering en bouwt voort op de vorige. Daarmee is projectorganisatie zichtbaar (wat doe je wanneer, waar ligt het resultaat) en is rapportage aan de beoordelaar gestructureerd. Een losse aanpak “alles in één keer bouwen zonder tussendocumenten” zou minder aantonen dat specificatie, ontwerp en realisatie op elkaar aansluiten.

---

## 3. Waarom UML, ERD en diagrammen in Markdown

UML en een ERD zijn gangbare manieren om ontwerp vast te leggen en te communiceren. Ze passen bij HBO-niveau en bij het vak: een klassendiagram maakt duidelijk welke objecten en verantwoordelijkheden in code horen; een ERD maakt datzelfde voor data en relaties. Diagrammen in Markdown met Mermaid zijn praktisch voor een Git-repository: ze zijn versieerbaar, geen losse binary die per ongeluk verouderd raakt, en ze zijn opnieuw te genereren of aan te passen (bijvoorbeeld via mermaid.live, zoals in `docs/diagrams/explanation.md`). Dat is verkennend onderzoek naar een passende, lichte manier van documenteren zonder zware tekenpakketten verplicht te maken.

---

## 4. Waarom ASP.NET Core MVC met Razor-views

De opdracht vraagt minimaal één webapplicatie in C#. ASP.NET Core MVC is een veelgebruikte, goed onderhouden stack van Microsoft: documentatie, voorbeelden en toekomstige onderhoudbaarheid zijn daarmee gunstig. Razor-views (`.cshtml`) zijn de standaardweergavelaag bij MVC: HTML met server-side logica waar nodig, zonder een aparte SPA-framework te introduceren. Voor een intern administratieprototype met overzichten en formulieren is dat doelmatig: snel schermen bouwen, duidelijke koppeling tussen controller-acties en pagina’s, en minder complexiteit dan een losse front-end en API alleen voor deze scope. De keuze sluit aan bij “eenvoudige webinterface” in het plan van aanpak.

---

## 5. Overige technische keuzes (kort onderbouwd)

- C# en .NET: passend bij de opleiding en de opdracht; sterke typering en gangbare tooling.
- SQLite: lokaal bestand, geen aparte database-server nodig voor demo en beoordeling; voldoende voor een prototype met beperkte data.
- ADO.NET zonder ORM: expliciete keuze uit de opdracht; SQL en data-toegang blijven zichtbaar in repositories, wat past bij het leerdoel database en bij controle op queries en veiligheid.
- Git: standaard voor versiebeheer; ondersteunt iteratief werken, rollback en zichtbare voortgang voor docent via commits en branches.

---

## 6. Ontwikkelomgeving: Visual Studio Code en vervolg naar Visual Studio

Voor dit project is gewerkt met Visual Studio Code. Dat is licht, snel op te starten en past goed bij het bewerken van Markdown-documentatie en Git naast C#-code.

Na overleg met mijn coach is het advies om voor het vervolg over te stappen naar Visual Studio (de volledige IDE van Microsoft, niet te verwarren met Visual Studio Code). Visual Studio is zwaarder en voelt voor sommige acties trager aan, maar biedt meer ingebouwde ondersteuning voor C# en .NET: denk aan debugging, projectbeheer, NuGet, testrunners en andere tools die voor grotere of langere C#-trajecten handig zijn. Dat geeft meer vrijheid en mogelijkheden specifiek rond C#-ontwikkeling, wat past bij verdere professionalisering na dit prototype.

---

## 7. Ethiek, duurzaamheid en interculturaliteit (kort)

Dit prototype gebruikt dummy-data: er worden geen echte persoonsgegevens van leden opgeslagen. Dat is passend voor een schoolopdracht en voorkomt onnodige privacy-risico’s. Duurzaamheid in technische zin is hier vooral: geen zware cloud-infrastructuur verplicht voor het draaien van de demo lokaal, en een stack die niet “weggooibaar” is na één vak (breed inzetbare kennis). Interculturaliteit speelt in dit concrete vakproduct beperkt; wel is de casus expliciet een Nederlandse verenigingscontext (voetbalclub), zodat taal en use cases aansluiten bij de doelgroep uit de analyse.

---

## 8. Conclusie

De gekozen tools en methoden sluiten aan op de opdracht (web, C#, relationele database, documentatie), op het plan van aanpak (stapsgewijs, naspeurbaar) en op gangbare professionele praktijk (versiebeheer, gestandaardiseerde ontwerpnotaties, MVC met Razor). Omdat er geen projectteam en geen echte externe opdrachtgever is, is professionele praktijk hier vooral zichtbaar in onderzoek en onderbouwing, heldere communicatie via documentatie en repository, en afstemming met begeleiders binnen de opleiding.
