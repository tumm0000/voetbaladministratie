# Portfolio: Design

Dit document beschrijft hoe het criterium Design uit het portfolio en uit het plan van aanpak (paragraaf 10.2) in dit project is ingevuld. Het sluit aan bij de competentie waarin staat dat je (delen van) systemen kunt ontwerpen, daarover kunt communiceren en het ontwerp kunt toetsen aan functionele en technische eisen én aan eisen rond gebruik en uitstraling, met gangbare methoden die bij deze challenge passen.

---

## 1. Criterium uit het plan van aanpak

Criterium: Je bent in staat (delen van) systemen te ontwerpen, hierover te communiceren en te valideren voor zowel functioneel/technische als esthetische eisen, met gangbare methoden die bij jouw challenge horen.

Toelichting in het plan: een gestructureerde aanpak, technische ontwerpmethoden zoals UML, een ontwerp dat functioneel/technisch én esthetisch dekt, heldere communicatie naar belanghebbenden, en vroeg valideren met bijvoorbeeld prototyping of feedback, gevolgd door verfijning.

In dit project komt dat samen in de analyse, het conceptueel model, het datamodel, het klassendiagram en de werkende webapplicatie met eenvoudige interface, zoals beschreven in de genoemde documenten onder `docs/` en in `docs/05-applicatie-toelichting.md`.

---

## 2. Gebruikersspecificaties

Gebruikersspecificaties beschrijven het verwachte gedrag van het systeem in termen van interactie tussen gebruiker en systeem. Ze zijn vastgelegd in de analysefase.

In dit project staan die specificaties in `docs/01-analyse.md`: basisfunctionaliteiten, use cases uit de verb–noun-analyse, korte use case-beschrijvingen (hoofd- en alternatieve stromen) en een overzicht met voorbeeldtests. Daarmee is vastgelegd wat bestuur en trainers van de applicatie mogen verwachten bij acties zoals lid registreren, betaling registreren en wedstrijd inplannen.

Validatie met uitvoerbare acceptatietests: de voorbeeldtests in hetzelfde document vormen het uitgangspunt voor acceptatie in de zin van “als de gebruiker dit doet, moet het systeem zo reageren”. In de realisatiefase worden die scenario’s uitgewerkt en uitgevoerd via het prototype; de uitkomsten en werkwijze staan toegelicht in `docs/05-applicatie-toelichting.md` (functionaliteit en testresultaten).

---

## 3. Softwareontwerp en diagrammen

Het ontwerp is vertaald naar diagrammen en een technische beschrijving die je kunt implementeren, met toelichting en onderbouwing van keuzes.

| Soort ontwerp | Waar vastgelegd | Korte inhoud |
|----------------|-----------------|--------------|
| Context en use cases | `docs/01-analyse.md`, diagrammen in `docs/diagrams/` | Actoren, subsystemen, use cases en context van het systeem |
| Conceptueel datamodel (ERD) | `docs/02-erd-conceptueel-model.md` | Entiteiten, relaties en korte toelichting |
| Relationeel datamodel | `docs/03-datamodel-sql.md`, `database/schema.sql` | Tabellen, sleutels en relaties voor opslag in de database |
| Object- en applicatieontwerp (UML) | `docs/04-klassendiagram.md` | Klassen, attributen en methoden die de kernfunctionaliteit ondersteunen |
| Applicatiestructuur | `docs/05-applicatie-toelichting.md` | Indeling in modellen, repositories, services, controllers en views (ASP.NET Core MVC) |

Keuzes die in de documenten worden toegelicht zijn onder meer: many-to-many tussen lid en team via een koppeltabel, aparte entiteiten voor contributie en betaling, en planning met conflictcontrole op veld en tijdslot. Die keuzes sluiten aan bij de analyse en zijn later in code doorgevoerd.

---

## 4. Functioneel, technisch en esthetisch

Functioneel en technisch: het ontwerp dekt ledenbeheer, contributie en betalingen, en wedstrijdplanning met beschikbaarheid en veldbezetting, zoals in de analyse en het datamodel is vastgelegd en in de applicatie is gebouwd met ADO.NET en SQLite (zonder ORM), in lijn met de randvoorwaarden uit het plan van aanpak.

Esthetisch en gebruik: de interface is bewust eenvoudig gehouden voor bestuur en trainers (overzichten, formulieren, duidelijke stromen), passend bij een prototype en bij de doelgroep zonder technische voorkennis.

---

## 5. Communiceren en valideren

Communicatie naar belanghebbenden verloopt via de documentatie in de repository, de README en de inlevering via Portflow. Validatie gebeurt door het ontwerp te vergelijken met de use cases en tests uit de analyse, door tussentijdse feedback van coach en docent, en door het uitvoeren van het prototype en het vastleggen van testresultaten in de applicatietoelichting.

---

## 6. Korte conclusie

Design in dit project is niet één plaatje, maar een keten: van gebruikersspecificaties en tests in de analyse, via ERD en datamodel naar UML en applicatiestructuur, met een eenvoudige webinterface en terugkoppeling via feedback en tests. Daarmee wordt invulling gegeven aan ontwerpen, communiceren en valideren rond functionele, technische en gebruikersgerichte eisen.
