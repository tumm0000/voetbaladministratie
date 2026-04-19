# Reflectie: Manage and control

Deze reflectie beschrijft hoe binnen dit project is gestuurd op voortgang, kwaliteit en controle, in lijn met criterium 10.4 Manage and control.

---

## 1. Criterium en toelichting

Criterium: Je voert activiteiten uit die gericht zijn op sturing, monitoring en optimalisatie van de ontwikkeling, ingebruikname en het gebruik van ICT-systemen, in relatie tot jouw challenge.

Toelichting: Binnen dit criterium staat centraal dat ontwikkeling iteratief verloopt, dat bestaande functionaliteit niet onnodig stukgaat, dat kwaliteit aantoonbaar bewaakt wordt met standaard technieken, en dat voortgang en keuzes traceerbaar zijn voor belanghebbenden.

---

## 2. Aanpak in dit project

De ontwikkeling is opgezet in stappen (analyse, ERD, datamodel, klassendiagram, implementatie), waarbij elke stap een concrete oplevering heeft in `docs/`, `database/` of `src/`. Door deze fasering is er sturing op inhoud, planning en kwaliteit.

---

## 3. Uitwerking per onderdeel

### 3.1 Continu

Ik werk iteratief per stap en per onderdeel, zodat wijzigingen beheersbaar blijven. Bestaande onderdelen blijven zoveel mogelijk stabiel terwijl nieuwe onderdelen worden toegevoegd. Wijzigingen worden bijgehouden via Git-commits per afgeronde stap, zodat zichtbaar is wat wanneer is aangepast.

### 3.2 Verbeteren

Ik gebruik standaard hulpmiddelen zoals versiebeheer (Git), documentatie per fase, en feedbackmomenten met coach en docent om kwaliteit te verbeteren. Op basis van feedback zijn keuzes aangescherpt, bijvoorbeeld rond datamodeldetails en technische aandachtspunten zoals veiligheid, waaronder het voorkomen van SQL-injectie in de implementatiefase.

### 3.3 Aantonen

Kwaliteit wordt onderbouwd met testbare use cases uit de analyse, controle van verwachte en onverwachte situaties tijdens implementatie, en een toelichting op testresultaten in de applicatiedocumentatie. Daarnaast maken de samenhang tussen ERD, datamodel en code inzichtelijk dat het ontwerp consequent is doorvertaald.

### 3.4 Standaard technieken en hulpmiddelen

In dit project worden gangbare technieken toegepast, waaronder Git voor versiebeheer en voortgangsbewaking, gestructureerde documentatie voor traceerbaarheid, en tests en validaties in de realisatiefase. Oplevering en communicatie verlopen via repository en Portflow, zodat beoordelaars en begeleiders de voortgang en kwaliteit kunnen volgen.

---

## 4. Codestructuur: waarom controllers, services, repositories en views

De webapplicatie volgt een indeling die je vaak ziet bij ASP.NET Core MVC-projecten: controllers en views voor de weblaag, daartussen een servicelaag voor de kernlogica, en repositories voor alles wat met de database te maken heeft. De precieze mappen staan toegelicht in `docs/05-applicatie-toelichting.md`.

Zo werkt het globaal samen:

- Een **controller** ontvangt een HTTP-verzoek (bijvoorbeeld een pagina openen of een formulier versturen), roept de juiste service aan en stuurt daarna een **view** terug met het model dat de pagina nodig heeft. De controller blijft daardoor dun: vooral routeren en doorgeven, weinig eigen regels.
- Een **service** bevat de stappen die bij een use case horen: valideren, samenstellen van resultaten, regels zoals “mag deze wedstrijd op dit veld op dit tijdstip”. Als die logica in de controller zou staan, wordt die snel groot en lastig te volgen.
- Een **repository** bevat alle SQL en ADO.NET-aanroepen voor één deel van het domein (bijvoorbeeld leden of wedstrijden). Databasecode zit daarmee op één plek in plaats van verspreid over de hele app. Dat helpt bij onderhoud en bij het consequent gebruiken van veilige queries (bijvoorbeeld parameters i.p.v. tekst in elkaar plakken).
- **Views** (Razor) zorgen alleen voor weergave: knoppen, tabellen, formulieren. Ze horen niet zelf naar de database te praten; ze krijgen data van buitenaf.

Waarom dit patroon zo vaak wordt gebruikt:

- Je maakt een duidelijk **scheiding tussen scherm, logica en data**. Als je later het uiterlijk aanpast, hoef je niet in de SQL te zitten; als je een databaseregel wijzigt, hoef je niet alle pagina’s langs.
- Het sluit aan op hoe Microsoft MVC uitlegt en hoe veel voorbeelden en cursussen zijn opgebouwd, dus anderen (en jij later) vinden snel hun weg in de code.
- Het maakt **testen en uitbreiden** eenvoudiger: je kunt services en repositories apart aanpakken en stap voor stap uitbreiden zonder meteen de hele applicatie om te gooien.

Die structuur past bij Manage and control omdat wijzigingen per laag beheersbaar blijven en de kwaliteit van de code beter te volgen is dan bij één grote door elkaar lopende bestanden.

---

## 5. Korte conclusie

Manage and control komt in dit project naar voren door een stapsgewijze aanpak, zichtbare voortgang via Git, gerichte feedbackmomenten en toetsing van keuzes in zowel documentatie als implementatie. De gekozen lagen in de code ondersteunen dat doordat verantwoordelijkheden duidelijk verdeeld zijn. Daarmee blijft de ontwikkeling bestuurbaar en wordt kwaliteit aantoonbaar bewaakt.
