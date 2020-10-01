/* DROP ALL TABLES
DROP SCHEMA Election CASCADE;
*/

CREATE SCHEMA IF NOT EXISTS Election;


DROP TABLE IF EXISTS Election.Election_Type;
CREATE TABLE IF NOT EXISTS Election.Election_Type
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL
);


DROP TABLE IF EXISTS Election.County_Election;
CREATE TABLE IF NOT EXISTS Election.County_Election
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    CountyId VARCHAR(50) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    ElectionDate DATE NOT NULL,
    ElectionTypeId INT NOT NULL,

    CONSTRAINT FK_ElectionType_CountyElection FOREIGN KEY (ElectionTypeId) REFERENCES Election.Election_Type(Id),
    CONSTRAINT FK_County_CountyElection FOREIGN KEY (CountyId)  REFERENCES Location.County(Id)
);

DROP TABLE IF EXISTS Election.Position_Level;
CREATE TABLE IF NOT EXISTS Election.Position_Level
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL


);


DROP TABLE IF EXISTS Election.Position_Level;
CREATE TABLE IF NOT EXISTS Election.Position_Level
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL


);


-- Insert rows into table 'Position_Level' in schema 'Election'
INSERT INTO Election.Position_Level
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

DROP TABLE IF EXISTS Election.Race_Type;
CREATE TABLE IF NOT EXISTS Election.Race_Type
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL,
    PositionLevelId INT NOT NULL,

    CONSTRAINT FK_PositionLevel_RaceType FOREIGN KEY (PositionLevelId)  REFERENCES Election.Position_Level(Id)
);


-- Insert rows into table 'Race_Type' in schema 'Election'
INSERT INTO Election.Race_Type
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
    'vernor', 2
),
(
    'State Senate', 2
),
(
    'State House of Representatives', 2
);


DROP TABLE IF EXISTS Election.Race;
CREATE TABLE IF NOT EXISTS Election.Race
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    CountyElectionId uuid NOT NULL,
    RaceTypeId INT NOT NULL,

    CONSTRAINT FK_CountyElection_Race FOREIGN KEY (CountyElectionId)  REFERENCES Election.County_Election(Id),
    CONSTRAINT FK_RaceType_Race FOREIGN KEY (RaceTypeId)  REFERENCES Election.Race_Type(Id)
);


DROP TABLE IF EXISTS Election.Voter_Turnout;
CREATE TABLE IF NOT EXISTS Election.Voter_Turnout
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    RegisteredVoters INT NOT NULL,
    TotalVoters INT NULL,
    RaceId uuid NOT NULL,

    CONSTRAINT FK_Race_VoterTurnout FOREIGN KEY (RaceId)  REFERENCES Election.Race(Id)
);

DROP TABLE IF EXISTS Election.Party;
CREATE TABLE IF NOT EXISTS Election.Party
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    "Name" VARCHAR(50) NOT NULL,
    Abbreviation VARCHAR(5) NULL
);


-- Insert rows into table 'Party' in schema 'Election'
INSERT INTO Election.Party
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


DROP TABLE IF EXISTS Election.Candidate;
CREATE TABLE IF NOT EXISTS Election.Candidate
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    "Name" VARCHAR(50) NOT NULL,
    PartyId INT NOT NULL,
    DateOfBirth DATE NOT NULL,

    CONSTRAINT FK_Party_Candidate FOREIGN KEY (PartyId)  REFERENCES Election.Party(Id)
);


DROP TABLE IF EXISTS Election.Result;
CREATE TABLE IF NOT EXISTS Election.Result
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    RaceId uuid NOT NULL,
    CandidateId INT NOT NULL,
    NumberOfVotesRecieved INT NOT NULL,
    Source VARCHAR(255) NOT NULL,

    CONSTRAINT FK_Race_Result FOREIGN KEY (RaceId)  REFERENCES Election.Race(Id),
    CONSTRAINT FK_Candidate_Result FOREIGN KEY (CandidateId)  REFERENCES Election.Candidate(Id)
);