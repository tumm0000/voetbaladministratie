# Stap 3: Datamodel en SQL-schema

In deze stap wordt het conceptueel model uit Stap 2 vertaald naar een relationeel datamodel met tabellen, kolommen en relaties. Het doel is een eenvoudig en consistent SQL-schema dat de kernfunctionaliteiten van het prototype ondersteunt.

---

## 1. Tabeloverzicht

| Tabel | Doel | Belangrijkste kolommen |
|-------|------|------------------------|
| **Lid** | Opslaan van ledengegevens | `lid_id`, `voornaam`, `achternaam`, `email`, `lidmaatschap_type`, `status` |
| **Team** | Opslaan van teaminformatie | `team_id`, `naam`, `categorie` |
| **LidTeam** | Koppelentiteit tussen leden en teams | `lid_team_id`, `lid_id`, `team_id`, `vanaf_datum` |
| **Contributie** | Vastleggen van contributieverplichtingen per lid | `contributie_id`, `lid_id`, `bedrag`, `vervaldatum`, `status` |
| **Betaling** | Vastleggen van ontvangen betalingen | `betaling_id`, `contributie_id`, `bedrag`, `betaaldatum`, `betaalmethode` |
| **Veld** | Opslaan van velden en locatie | `veld_id`, `naam`, `locatie` |
| **Beschikbaarheid** | Beschikbaarheid van leden voor planning | `beschikbaarheid_id`, `lid_id`, `datum`, `tijdslot`, `status` |
| **Wedstrijd** | Opslaan van geplande wedstrijden | `wedstrijd_id`, `team_id`, `veld_id`, `datum`, `tijdslot`, `status` |
| **Training** | Opslaan van geplande trainingen | `training_id`, `team_id`, `veld_id`, `datum`, `tijdslot`, `status` |

---

## 2. Relaties en integriteit

- De many-to-many-relatie tussen **Lid** en **Team** is uitgewerkt via **LidTeam**.
- Een **Contributie** hoort bij precies een **Lid**; een lid kan meerdere contributieregels hebben.
- Een **Betaling** hoort bij precies een **Contributie**; een contributie kan in meerdere betalingen worden voldaan.
- **Wedstrijd** en **Training** verwijzen naar zowel een **Team** als een **Veld**.
- **Beschikbaarheid** verwijst naar een **Lid** en ondersteunt de planningslogica.
- Met primary keys en foreign keys blijft de referentiële integriteit behouden.
- Met eenvoudige `CHECK`-constraints op bedragen (`> 0`) worden ongeldige waarden beperkt.

---

## 3. Korte toelichting

Dit datamodel ondersteunt de gevraagde opslag van lidmaatschappen, wedstrijden en betalingen binnen de afgesproken scope van het project.  
Het SQL-script in `database/schema.sql` maakt alle tabellen en relaties aan die nodig zijn voor het prototype.  
De gekozen structuur is bewust eenvoudig gehouden, zodat deze goed aansluit op de opdracht, de ERD uit Stap 2 en de latere implementatie in C# met ADO.NET (zonder ORM).
