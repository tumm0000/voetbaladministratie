# Plan van aanpak: Slimme Voetbalclub Administratie

## 1. Projectgegevens

| Onderdeel | Beschrijving |
|-----------|---------------|
| **Projectnaam** | Slimme Voetbalclub Administratie – Software voor Ledenbeheer en Wedstrijdplanning |
| **Opdracht** | Individuele opdracht: ontwerp en werkend prototype voor een administratiesysteem voor een lokale voetbalclub |
| **Context** | De club heeft moeite met handmatig ledenbeheer, contributie-inning en wedstrijdplanning; er is behoefte aan een softwareoplossing die deze processen ondersteunt |

---

## 2. Doelstelling

Het doel is een **eenvoudige, werkende softwareoplossing** te ontwerpen en te bouwen die:

- **Leden en contributies beheert** — inclusief automatische herinneringen bij achterstallige betalingen  
- **Wedstrijden plant** — met inachtneming van beschikbaarheid van spelers en velden  
- **Een database bijhoudt** — voor teamindelingen, trainingen en wedstrijden  
- **Een eenvoudige interface biedt** — zodat bestuursleden en trainers informatie kunnen inzien en aanpassen  

Het eindresultaat is een prototype dat deze functionaliteit demonstreert, ondersteund door analyse, ontwerp en documentatie.

---

## 3. Aanleiding en probleemstelling

Een lokale voetbalclub beheert leden, contributies en wedstrijdplanning nu handmatig. Dat is tijdrovend en foutgevoelig. Door een gerichte softwareoplossing te bouwen wordt:

- inzicht in betalingen en achterstalligheid vergroot  
- planning van wedstrijden en trainingen vereenvoudigd  
- informatie over leden en teams centraal en consistent beheerd  

Het project doorloopt stap voor stap het ontwikkelproces (analyse → conceptueel model → database → ontwerp → implementatie) en levert naast een werkende applicatie ook de bijbehorende documenten.

---

## 4. Scope

**Binnen scope:**

- Ledenregistratie en -beheer (gegevens, lidmaatschap)  
- Contributie en betalingen registreren; bepalen van achterstallige betalingen en (voorbereiding voor) herinneringen  
- Wedstrijd- en (optioneel) trainingsplanning met beschikbaarheid van spelers en velden  
- Database voor leden, teams, wedstrijden, trainingen, velden, betalingen  
- Eenvoudige webinterface voor bestuur en trainers  
- Documentatie: analyse, ERD, datamodel, klassendiagram, toelichting applicatie  

**Buiten scope (voor dit prototype):**

- Volledige e-mail-/SMS-verzending van herinneringen (wel: logica om achterstalligen te bepalen)  
- Geavanceerde rechten en rollen (eenvoudige toegang volstaat)  
- Koppelingen met externe systemen (betaalproviders, bonden)  
- Mobiele app; een responsive webinterface is voldoende  

---

## 5. Aanpak en methodiek

De aanpak volgt de **vijf opdrachtstappen** en sluit aan bij een gestructureerd ontwikkelproces:

1. **Analyse** — Wat moet het systeem doen? Functionaliteiten, benodigde gegevens, contextdiagram.  
2. **Conceptueel model** — Welke data wordt opgeslagen? ERD met entiteiten en relaties.  
3. **Databaseontwerp** — Hoe wordt de data opgeslagen? Tabellen, SQL-script, korte toelichting.  
4. **Klassendiagram** — Hoe is de code gestructureerd? UML met objecten en methoden.  
5. **Werkende applicatie** — Prototype in C# (webapplicatie) met dummy-data, planninglogica, database-updates en eenvoudige UI.  

**Technische uitgangspunten:**

- **Programmeertaal:** C#  
- **Applicatie:** minimaal één webapplicatie
- **Database:** toegang via ADO.NET of eigen data-laag; **geen Entity Framework** (zoals in de opdracht bepaald)  
- **Versiebeheer:** code en documentatie in Git
- **Oplevering:** documentatie via Portflow; code via link naar de repository en eventueel een zip  

Documenten worden in de map `docs/` bijgehouden; het SQL-schema in `database/`; de applicatie in `src/`.

---

## 6. Fasering en planning

De volgorde is lineair: elke stap bouwt voort op de vorige.

| Fase | Activiteit | Oplevering |
|------|------------|------------|
| **0** | Repo opzetten, .gitignore, README, eerste commit (en eventueel push) | Werkende Git-repository |
| **Stap 1** | Analyse: functionaliteiten, gegevens, contextdiagram | `docs/01-analyse.md` |
| **Stap 2** | Conceptueel model: ERD en toelichting (max. 1 pagina) | `docs/02-erd-conceptueel-model.md` |
| **Stap 3** | Databaseontwerp: tabellen, SQL-script, toelichting (max. 1 pagina) | `database/schema.sql`, `docs/03-datamodel-sql.md` |
| **Stap 4** | Klassendiagram: UML en toelichting (max. 1 pagina) | `docs/04-klassendiagram.md` |
| **Stap 5** | Implementatie: C#-webapplicatie, dummy-data, planninglogica, UI, toelichting (max. 1 pagina) | `src/VoetbalAdministratie/`, `docs/05-applicatie-toelichting.md` |
| **Afronding** | README bijwerken, tag voor levering, documentatie en codelink in Portflow | Compleet ingeleverd project |

Na elke stap wordt gecommit (en waar mogelijk gepusht) zodat de voortgang in de repository zichtbaar is.

---

## 7. Randvoorwaarden

- **Individueel** — De opdracht wordt individueel uitgevoerd.  
- **Geen ORM** — Entity Framework (en vergelijkbare ORM’s) zijn niet toegestaan; database-toegang via ADO.NET of eigen SQL-laag.  
- **Minimaal één webapplicatie** — In lijn met de leeruitkomsten wordt ten minste één webapplicatie gerealiseerd.  
- **Toegang tot repository** — Belanghebbenden (docent/beoordelaar) krijgen toegang tot de Git-repository.  
- **Levering** — Documentatie wordt via Portflow ingeleverd; code via een link naar de repository en eventueel een zip-bestand.

---

## 8. Risico’s en beperkingen

- **Tijd** — Het prototype dekt de kernfunctionaliteit; uitbreidingen (zoals daadwerkelijke e-mailverzending) kunnen in een vervolgfase.  
- **Omgeving** — De applicatie wordt lokaal en/of op een eenvoudige hostingomgeving gedraaid; installatie-instructies worden in de toelichting en README beschreven.  
- **Data** — Voor demonstratie wordt gebruikgemaakt van dummy-/seed-data; echte clubgegevens zijn niet noodzakelijk voor de opdracht.

---

## 9. Communicatie en versiebeheer

- **Versiebeheer:** Git.  
- **Commitberichten:** Duidelijk en gerelateerd aan de stap (bijv. "Stap 1: Analyse en contextdiagram").  
- **Push:** Na elke afgeronde stap naar de remote repository pushen.  
- **Levering:** Documentatie in Portflow en link naar Git repository.

---

## 10. Portflow: zes beoordelingscriteria

Naast de projecteigen eisen wordt het portfolio beoordeeld op de onderstaande zes criteria. Hieronder staan de definitie van elk criterium en hoe dit project daaraan bijdraagt.

### 10.1 Analysis

**Criterium:** Je bereikt gewenste resultaten door processen, producten en informatiestromen op een methodische en grondige manier te analyseren.

**Toelichting:** Je weet wat je onderzoekt en waarom (met oog voor opdrachtgever, doelgroep en markt); je gebruikt meerdere onderbouwde onderzoeksmethoden (o.a. DOT-framework); je past verschillende analysemethoden toe of richt je op meerdere aspecten (bijv. behoeften opdrachtgever/doelgroep, vergelijkbare producten, expertkennis, literatuur).

**In dit project:** De analysefase (Stap 1) met `docs/01-analyse.md`: basisfunctionaliteiten, benodigde gegevens en contextdiagram. De opdrachtgever (voetbalclub) en doelgroep (bestuur, trainers) zijn expliciet meegenomen; de informatiestromen komen terug in het contextdiagram en de gegevensanalyse.

---

### 10.2 Design

**Criterium:** Je bent in staat (delen van) systemen te ontwerpen, hierover te communiceren en te valideren voor zowel functioneel/technische als esthetische eisen, met gangbare methoden die bij jouw challenge horen.

**Toelichting:** Gestructureerde en methodische aanpak; conceptontwikkeling (divergeren/convergeren, brainstorms, bestaande theorieën) en gestructureerde technische ontwerpmethoden (UML, RUP); het ontwerp dekt functioneel/technisch én esthetisch ontwerp; je communiceert het ontwerp helder naar belanghebbenden; je valideert het ontwerp vroeg (bijv. paper prototyping) en verfijnt op basis van resultaten.

**In dit project:** ERD (Stap 2), datamodel en SQL-schema (Stap 3), UML-klassendiagram (Stap 4). Ontwerp wordt vastgelegd en toegelicht in `docs/02-erd-conceptueel-model.md`, `docs/03-datamodel-sql.md` en `docs/04-klassendiagram.md`. De eenvoudige UI sluit aan bij esthetische/gebruikerseisen voor bestuur en trainers.

---

### 10.3 Realization

**Criterium:** Je implementeert en valideert een product op basis van een ontwerp op een gestructureerde manier, met innovatieve en/of relevante technologie, in lijn met kwaliteitseisen binnen de context van jouw challenge.

**Toelichting:** Valideer product en kwaliteit gestructureerd (bijv. user testing, expert reviews, code/peer reviews, (unit)tests); werk volgens gangbare ontwikkelprocessen (Scrum/Agile, Kanban); gebruik innovatieve/relevante technologie die past bij de behoefte; het product voldoet aan vooraf gedefinieerde kwaliteitseisen (o.a. stabiliteit, performance, security).

**In dit project:** Werkende C#-webapplicatie (Stap 5) in `src/VoetbalAdministratie/`: implementatie volgens ontwerp (ERD, datamodel, klassendiagram), zonder ORM. Toelichting en testresultaten in `docs/05-applicatie-toelichting.md`. Versiebeheer (Git) en duidelijke commit-structuur ondersteunen de gestructureerde aanpak.

---

### 10.4 Manage and control

**Criterium:** Je voert activiteiten uit die gericht zijn op sturing, monitoring en optimalisatie van de ontwikkeling, ingebruikname en het gebruik van ICT-systemen, in relatie tot jouw challenge.

**Toelichting:** Sturing, monitoring en optimalisatie van de ontwikkeling op een gestructureerde manier opbouwen; communiceren met belanghebbenden over deze activiteiten.

**In dit project:** Plan van aanpak (dit document) stuurt de aanpak. Git wordt gebruikt voor controle en voortgang. Afronding: versie 1.0 in Github voor oplevering; overdracht documentatie en code via Portflow; toegang repository voor docent als communicatie met belanghebbenden.

---

### 10.5 Professional standard

**Criterium:** Je past professionele praktijken toe, zowel individueel als in teams, op het gebied van projectorganisatie, communicatie met belanghebbenden, verkennend onderzoek en rapportage.

**In dit project:** Individuele uitvoering met duidelijke projectorganisatie (fasering, deliverables). Communicatie met belanghebbenden via documentatie (analyse, ontwerp, toelichting) en toegang tot de repository. Rapportage via de documenten in `docs/` en de README; analyse en ontwerp vormen het verkennend onderzoek.

---

### 10.6 Personal leadership
 
**Criterium:** Je neemt het initiatief om feedback te vragen en hierop te reflecteren. Je identificeert je eigen kernwaarden als basis voor je studiecarrière en professionele ontwikkeling.

**In dit project:** Dit criterium wordt niet door één specifiek document afgedekt; het gaat om je eigen handelen. Hiervoor zal ik feedback vragen aan docenten en daarop reflecteren via FeedPulse. En de feedback/rubric van Portflow gebruiken om eventuele aanpassingen te maken en de juiste richting op te gaan met dit project.
