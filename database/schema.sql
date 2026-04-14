-- Stap 3: Databaseontwerp - Slimme Voetbalclub Administratie
-- Dit schema volgt het conceptueel model uit Stap 2.

CREATE TABLE Lid (
    lid_id INT PRIMARY KEY,
    voornaam VARCHAR(100) NOT NULL,
    achternaam VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL,
    telefoon VARCHAR(20),
    lidmaatschap_type VARCHAR(50) NOT NULL,
    status VARCHAR(20) NOT NULL
);

CREATE TABLE Team (
    team_id INT PRIMARY KEY,
    naam VARCHAR(100) NOT NULL,
    categorie VARCHAR(50) NOT NULL
);

CREATE TABLE LidTeam (
    lid_team_id INT PRIMARY KEY,
    lid_id INT NOT NULL,
    team_id INT NOT NULL,
    vanaf_datum DATE NOT NULL,
    CONSTRAINT fk_lidteam_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id),
    CONSTRAINT fk_lidteam_team FOREIGN KEY (team_id) REFERENCES Team(team_id)
);

CREATE TABLE Contributie (
    contributie_id INT PRIMARY KEY,
    lid_id INT NOT NULL,
    bedrag DECIMAL(10,2) NOT NULL,
    vervaldatum DATE NOT NULL,
    status VARCHAR(20) NOT NULL,
    CONSTRAINT chk_contributie_bedrag CHECK (bedrag > 0),
    CONSTRAINT fk_contributie_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id)
);

CREATE TABLE Betaling (
    betaling_id INT PRIMARY KEY,
    contributie_id INT NOT NULL,
    bedrag DECIMAL(10,2) NOT NULL,
    betaaldatum DATE NOT NULL,
    betaalmethode VARCHAR(50),
    CONSTRAINT chk_betaling_bedrag CHECK (bedrag > 0),
    CONSTRAINT fk_betaling_contributie FOREIGN KEY (contributie_id) REFERENCES Contributie(contributie_id)
);

CREATE TABLE Veld (
    veld_id INT PRIMARY KEY,
    naam VARCHAR(100) NOT NULL,
    locatie VARCHAR(150)
);

CREATE TABLE Beschikbaarheid (
    beschikbaarheid_id INT PRIMARY KEY,
    lid_id INT NOT NULL,
    datum DATE NOT NULL,
    tijdslot VARCHAR(30) NOT NULL,
    status VARCHAR(20) NOT NULL,
    CONSTRAINT fk_beschikbaarheid_lid FOREIGN KEY (lid_id) REFERENCES Lid(lid_id)
);

CREATE TABLE Wedstrijd (
    wedstrijd_id INT PRIMARY KEY,
    team_id INT NOT NULL,
    veld_id INT NOT NULL,
    datum DATE NOT NULL,
    tijdslot VARCHAR(30) NOT NULL,
    tegenstander VARCHAR(100),
    status VARCHAR(20) NOT NULL,
    CONSTRAINT fk_wedstrijd_team FOREIGN KEY (team_id) REFERENCES Team(team_id),
    CONSTRAINT fk_wedstrijd_veld FOREIGN KEY (veld_id) REFERENCES Veld(veld_id)
);

CREATE TABLE Training (
    training_id INT PRIMARY KEY,
    team_id INT NOT NULL,
    veld_id INT NOT NULL,
    datum DATE NOT NULL,
    tijdslot VARCHAR(30) NOT NULL,
    status VARCHAR(20) NOT NULL,
    CONSTRAINT fk_training_team FOREIGN KEY (team_id) REFERENCES Team(team_id),
    CONSTRAINT fk_training_veld FOREIGN KEY (veld_id) REFERENCES Veld(veld_id)
);
