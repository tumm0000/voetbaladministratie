# Stap 2: Conceptueel model (ERD)

In deze stap wordt vastgelegd welke kerngegevens de applicatie opslaat en hoe deze gegevens met elkaar samenhangen. Het doel is een helder conceptueel model dat aansluit op de analyse en als basis dient voor het datamodel in Stap 3.

---

## 1. Entiteiten en relaties

De belangrijkste entiteiten uit de analyse zijn:

- **Lid** - persoonsgegevens en lidmaatschapsgegevens
- **Team** - teamnaam en categorie
- **LidTeam** - koppeling tussen leden en teams (many-to-many)
- **Contributie** - verschuldigde contributie per periode
- **Betaling** - geregistreerde betalingen van contributies
- **Veld** - beschikbare speelvelden
- **Beschikbaarheid** - beschikbaarheid van een lid op datum/tijd
- **Wedstrijd** - ingeplande wedstrijd met datum, tijd en status
- **Training** - ingeplande training met datum en tijd

---

## 2. ERD (conceptueel)

```mermaid
erDiagram
    LID ||--o{ LID_TEAM : is_ingedeeld_in
    TEAM ||--o{ LID_TEAM : bevat

    LID ||--o{ CONTRIBUTIE : heeft
    CONTRIBUTIE ||--o{ BETALING : wordt_betaald_met

    LID ||--o{ BESCHIKBAARHEID : geeft_op
    VELD ||--o{ WEDSTRIJD : host
    VELD ||--o{ TRAINING : host

    TEAM ||--o{ WEDSTRIJD : speelt
    TEAM ||--o{ TRAINING : traint

    LID {
        int lid_id PK
        string voornaam
        string achternaam
        string email
        string telefoon
        string lidmaatschap_type
        string status
    }

    TEAM {
        int team_id PK
        string naam
        string categorie
    }

    LID_TEAM {
        int lid_team_id PK
        int lid_id FK
        int team_id FK
        date vanaf_datum
    }

    CONTRIBUTIE {
        int contributie_id PK
        int lid_id FK
        decimal bedrag
        date vervaldatum
        string status
    }

    BETALING {
        int betaling_id PK
        int contributie_id FK
        decimal bedrag
        date betaaldatum
        string betaalmethode
    }

    VELD {
        int veld_id PK
        string naam
        string locatie
    }

    BESCHIKBAARHEID {
        int beschikbaarheid_id PK
        int lid_id FK
        date datum
        string tijdslot
        string status
    }

    WEDSTRIJD {
        int wedstrijd_id PK
        int team_id FK
        int veld_id FK
        date datum
        string tijdslot
        string tegenstander
        string status
    }

    TRAINING {
        int training_id PK
        int team_id FK
        int veld_id FK
        date datum
        string tijdslot
        string status
    }
```

---

## 3. Korte toelichting

- Een **lid** kan in meerdere teams zitten en een **team** bestaat uit meerdere leden; daarom is er een koppelentiteit **LidTeam**.
- Per lid worden een of meer **contributies** vastgelegd. Een contributie kan in een of meerdere termijnen worden betaald via **betalingen**.
- **Wedstrijden** en **trainingen** zijn gekoppeld aan een team en worden op een veld gepland.
- **Beschikbaarheid** wordt per lid opgeslagen zodat planning rekening kan houden met aanwezigheid.
- Dit model blijft binnen de afgesproken scope: kernadministratie voor leden, contributie en planning, zonder externe koppelingen of uitgebreide rollenstructuur.

Dit ERD vormt de basis voor het relationele datamodel en het SQL-schema in Stap 3.
