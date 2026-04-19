# Portfolio: Realisatie

Dit document beschrijft hoe realisatie in dit project is ingevuld. Het sluit aan bij het Portflow-criterium Realization (je implementeert en valideert een product op basis van een ontwerp op een gestructureerde manier, met relevante technologie en aandacht voor kwaliteit, binnen de context van je challenge) en bij de leeruitkomst Realiseren zoals die in de opleiding wordt uitgewerkt: herhaaldelijk bouwen en onderhouden, aansluiten op ontwerp en specificaties, opleveren voor gebruik, veiligheid, onderhoudbaarheid en objectgeoriënteerd werken.

---

## 1. Criterium uit het plan van aanpak (10.3)

Criterium: Je implementeert en valideert een product op basis van een ontwerp op een gestructureerde manier, met innovatieve en/of relevante technologie, in lijn met kwaliteitseisen binnen de context van jouw challenge.

Toelichting in het plan: product en kwaliteit gestructureerd valideren (bijvoorbeeld testen, reviews, unit-tests); werken volgens gangbare ontwikkelprocessen; technologie kiezen die past bij de behoefte; voldoen aan kwaliteitseisen zoals stabiliteit, performance en security.

In dit project: een werkende ASP.NET Core MVC-webapplicatie in C#, gebouwd volgens het ontwerp (ERD, datamodel, klassendiagram), met database via ADO.NET zonder ORM. Toelichting en testresultaten staan in `docs/05-applicatie-toelichting.md`. Versiebeheer met Git en duidelijke commits ondersteunen de gestructureerde aanpak.

---

## 2. Realiseren: uitwerking per onderdeel

### 2.1 Herhaaldelijk bouwen, uitbreiden en onderhouden

Dit portfolio-project doorloopt vaste stappen (analyse, ontwerp, database, code). Binnen de realisatiefase is het prototype stap voor stap uitgebreid: eerst kernschermen en data, daarna extra logica (bijvoorbeeld planning en conflicten). Wijzigingen zijn vastgelegd in Git, zodat zichtbaar is hoe de software in de loop van de tijd is gegroeid. Dat komt overeen met het idee dat je software niet één keer “af” maakt, maar iteratief uitbreidt en bijstuurt.

### 2.2 Ontwerp en specificaties opnieuw meenemen

Uitbreiden en onderhouden begint bij het teruglezen van wat er al vastligt. Bij aanpassingen in code of database sluit de implementatie aan op `docs/01-analyse.md` (gedrag en tests), `docs/02-erd-conceptueel-model.md`, `docs/03-datamodel-sql.md` en `docs/04-klassendiagram.md`. Als tijdens bouwen of feedback blijkt dat iets anders moet (bijvoorbeeld randgevallen rond teams of planning), wordt dat eerst helder in het gesprek of in de documentatie en daarna in de code doorgevoerd. Zo blijft realisatie gekoppeld aan specificatie en ontwerp, in plaats van losse improvisatie.

### 2.3 Opleveren voor belanghebbenden

De software staat beschikbaar via de repository en de instructies in `README.md`: met de .NET SDK en één commando is de webapp lokaal te starten. De database wordt bij eerste start aangemaakt en gevuld met dummy-data, zodat een docent of beoordelaar het systeem direct kan uitproberen. Documentatie en link naar de code horen bij de oplevering via Portflow, zodat stakeholders het product kunnen gebruiken en beoordelen.

### 2.4 Veilig

Veiligheid heeft twee kanten die in de realisatie zijn meegenomen.

Onbedoelde fouten door gebruik op een manier die niet in alle detail was uitgeschreven: invoer wordt in de applicatie verwerkt via controllers en services; waar nodig hoort validatie en duidelijke foutafhandeling bij (bijvoorbeeld ontbrekende gegevens of geen toegang tot een record). Daarmee wordt voorkomen dat het systeem stil valt of onduidelijk gedrag toont zonder uitleg.

Onverwachte fouten door omgeving of techniek: database-acties lopen via ADO.NET in repositories. Bij problemen met de verbinding of uitvoering van SQL hoort nette afhandeling (geen onnodige stacktrace naar de eindgebruiker). Daarnaast is aandacht besteed aan het veilig verwerken van parameters in queries om risico’s zoals SQL-injectie te beperken; dat past bij security als kwaliteitseis.

### 2.5 Onderhoudbaar

De code is opgedeeld in controllers, services, repositories, modellen en views (zie ook `docs/07-manage-and-control-reflectie.md`). Daardoor is het eenvoudiger om later onderdelen aan te passen zonder de hele applicatie door elkaar te halen. Het klassendiagram en de mapstructuur in `docs/05-applicatie-toelichting.md` maken duidelijk waar nieuwe functionaliteit het beste past. Dat ondersteunt onderhoud en uitbreiding wanneer er nieuwe eisen bij komen.

### 2.6 Objectgeoriënteerd en onderbouwd

Het systeem is opgebouwd uit klassen per domein (bijvoorbeeld lid, team, contributie) en aparte klassen voor data-toegang en bedrijfslogica. Dat sluit aan op gangbare OO-principes zoals scheiding van verantwoordelijkheden en het groeperen van gedrag bij data. De keuze is onderbouwd in `docs/04-klassendiagram.md` en in de beschrijving van de lagen in stap 5.

---

## 3. Technologie en kwaliteit

Voor deze challenge is gekozen voor een webapplicatie (minimaal één webapplicatie volgens de opdracht), ASP.NET Core MVC en C#, met SQLite en ADO.NET. Dat is geen experimentele stack maar wel passend en gangbaar voor een administratieprototype: snel lokaal te draaien, goed gedocumenteerd door Microsoft en geschikt om relationele data en CRUD te tonen zonder ORM, zoals de opdracht voorschrijft.

Kwaliteit is niet alleen “het compileert”, maar ook aansluiting op use cases en tests uit de analyse en het vastleggen van testresultaten in `docs/05-applicatie-toelichting.md`. Daarmee is realisatie expliciet gekoppeld aan validatie.

---

## 4. Korte conclusie

Realisatie in dit project betekent: bouwen volgens ontwerp en analyse, iteratief uitbreiden met Git, webapp plus database voor stakeholders uitvoerbaar maken, en aandacht voor veiligheid, onderhoudbaarheid en OO-principes. Validatie gebeurt gestructureerd via testscenario’s en documentatie, in lijn met criterium 10.3 en de leeruitkomst Realiseren.
