DROP TABLE IF EXISTS Election_Type;
CREATE TABLE IF NOT EXISTS Election_Type
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL
);


DROP TABLE IF EXISTS County_Election;
CREATE TABLE IF NOT EXISTS County_Election
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    CountyId VARCHAR(50) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    ElectionDate DATE NOT NULL,
    ElectionTypeId INT NOT NULL,

    CONSTRAINT FK_ElectionType_CountyElection FOREIGN KEY (ElectionTypeId) REFERENCES Election_Type(Id),
    CONSTRAINT FK_County_CountyElection FOREIGN KEY (CountyId)  REFERENCES County(Id)
);

DROP TABLE IF EXISTS Position_Level;
CREATE TABLE IF NOT EXISTS Position_Level
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL


);


DROP TABLE IF EXISTS Position_Level;
CREATE TABLE IF NOT EXISTS Position_Level
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL


);


-- Insert rows into table 'Position_Level' in schema 'Election'
INSERT INTO Position_Level
( -- Columns to insert data into
    Description
)
VALUES
(
    'National'
),
(
    'State'
),
(
    'County'
),
(
    'Unknown'
);

DROP TABLE IF EXISTS ElectionRace_Type;
CREATE TABLE IF NOT EXISTS ElectionRace_Type
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL,
    PositionLevelId INT NOT NULL,

    CONSTRAINT FK_PositionLevel_ElectionRaceType FOREIGN KEY (PositionLevelId)  REFERENCES Position_Level(Id)
);


-- Insert rows into table 'ElectionRace_Type' in schema 'Election'
INSERT INTO ElectionRace_Type
( -- Columns to insert data into
    Description, PositionLevelId
)
VALUES
(
    'President of the United States of America', 1
),
( -- First row: values for the columns in the list above
    'United State Senate', 1
),
( -- Second row: values for the columns in the list above
    'United State House of Representatives', 1
),
(
    'Governor', 2
),
(
    'State Senate', 2
),
(
    'State House of Representatives', 2
);


DROP TABLE IF EXISTS ElectionRace;
CREATE TABLE IF NOT EXISTS ElectionRace
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    CountyElectionId uuid NOT NULL,
    ElectionRaceTypeId INT NOT NULL,

    CONSTRAINT FK_CountyElection_ElectionRace FOREIGN KEY (CountyElectionId)  REFERENCES County_Election(Id),
    CONSTRAINT FK_ElectionRaceType_ElectionRace FOREIGN KEY (ElectionRaceTypeId)  REFERENCES ElectionRace_Type(Id)
);


DROP TABLE IF EXISTS Voter_Turnout;
CREATE TABLE IF NOT EXISTS Voter_Turnout
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    RegisteredVoters INT NOT NULL,
    TotalVoters INT NULL,
    ElectionRaceId uuid NOT NULL,

    CONSTRAINT FK_ElectionRace_VoterTurnout FOREIGN KEY (ElectionRaceId)  REFERENCES ElectionRace(Id)
);

DROP TABLE IF EXISTS Party;
CREATE TABLE IF NOT EXISTS Party
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    "Name" VARCHAR(50) NOT NULL,
    Abbreviation VARCHAR(5) NULL
);


-- Insert rows into table 'Party' in schema 'Election'
INSERT INTO Party
( -- Columns to insert data into
 "Name", Abbreviation
)
VALUES
( -- First row: values for the columns in the list above
    'Democratic Party', 'D'
),
( -- Second row: values for the columns in the list above
    'Republic Party', 'R'
),
( -- Second row: values for the columns in the list above
    'Libertarian Party', 'L'
),
(
    'Green Party', 'G'
),
(
    'Vermont Progressive Party', 'VPP'
),
(
    'Working Families Party', 'WFP'
),
(   
    'Independence Party of New York', 'IPNY'
),
(
    'Reform Party', 'RP'
);


DROP TABLE IF EXISTS Candidate;
CREATE TABLE IF NOT EXISTS Candidate
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Name VARCHAR(50) NOT NULL,
    PartyId INT NOT NULL,
    DateOfBirth DATE NOT NULL,

    CONSTRAINT FK_Party_Candidate FOREIGN KEY (PartyId)  REFERENCES Party(Id)
);


DROP TABLE IF EXISTS Result;
CREATE TABLE IF NOT EXISTS Result
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    ElectionRaceId uuid NOT NULL,
    CandidateId INT NOT NULL,
    NumberOfVotesRecieved INT NOT NULL,
    ResultResolution VARCHAR(8) NOT NULL,
    Precinct VARCHAR(255) NOT NULL,
    Source VARCHAR(255) NOT NULL,

    CONSTRAINT FK_ElectionRace_Result FOREIGN KEY (ElectionRaceId)  REFERENCES ElectionRace(Id),
    CONSTRAINT FK_Candidate_Result FOREIGN KEY (CandidateId)  REFERENCES Candidate(Id)
);