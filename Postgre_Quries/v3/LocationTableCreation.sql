/*
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
ALTER SCHEMA public OWNER TO stopgerry;
GRANT USAGE ON SCHEMA public TO cis625;
GRANT USAGE ON ALL TABLES IN SCHEMA public TO cis625;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO cis625;
CREATE EXTENSION postgis;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

*/

DROP TABLE IF EXISTS Block;
CREATE TABLE IF NOT EXISTS Block
(
 Id          VARCHAR(50) NOT NULL PRIMARY KEY,
 Description VARCHAR(255) NOT NULL,
 Source      VARCHAR(255) NOT NULL,
 Coordinates geometry NOT NULL,
 Border      geometry NULL

);

DROP TABLE IF EXISTS County;
CREATE TABLE IF NOT EXISTS County
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    Description VARCHAR(255) NOT NULL,
    StateId INT NOT NULL
    Source text NOT NULL,
    Border geometry NULL

);


DROP TABLE IF EXISTS State;
CREATE TABLE IF NOT EXISTS State
(
    Id INT NOT NULL PRIMARY KEY, -- Primary Key column
    Name VARCHAR(50) NOT NULL,
    Abbreviation VARCHAR(2) NOT NULL,
    Source text NOT NULL,
    StateType VARCHAR(20) NOT NULL,
    CountyType INT NOT NULL,
    Border GEOMETRY NULL,
    
);

DROP TABLE IF EXISTS State_Time;
CREATE TABLE IF NOT EXISTS State_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    StateId INT NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    

    CONSTRAINT FK_StateTime_State FOREIGN KEY (StateId)  REFERENCES State(Id)
);

DROP TABLE IF EXISTS County_Time;
CREATE TABLE IF NOT EXISTS County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    CountyId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    StateId INT NOT NULL,

    CONSTRAINT FK_State_County_CountyId FOREIGN KEY (CountyId)  REFERENCES County(Id),
    CONSTRAINT FK_State_County_StateId FOREIGN KEY (StateId)  REFERENCES State(Id)
);

DROP TABLE IF EXISTS Block_County_Time;
CREATE TABLE IF NOT EXISTS Block_County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    BlockId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    CountyId VARCHAR(50) NOT NULL,

    CONSTRAINT FK_County_Block_BlockId FOREIGN KEY (BlockId)  REFERENCES Block(Id),
    CONSTRAINT FK_County_Block_CountyId FOREIGN KEY (CountyId)  REFERENCES County(Id)
);

DROP TABLE IF EXISTS District;
CREATE TABLE IF NOT EXISTS District
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    Year INT NOT NULL,
    DistrictCode VARCHAR(6) NOT NULL,
    Description text NOT NULL,
    Source text NOT NULL,
    DistrictTypeId INT NOT NULL,
    Border geometry NULL,


    CONSTRAINT FK_DistrictType_District FOREIGN KEY (DistrictTypeId)  REFERENCES DistrictType(Id)
);

DROP TABLE IF EXISTS District_Time;
CREATE TABLE IF NOT EXISTS District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    DistrictId VARCHAR(50) NOT NULL,
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,

);


DROP TABLE IF EXISTS Block_District_Time;
CREATE TABLE IF NOT EXISTS Block_District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    BlockId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    DistrictId uuid NOT NULL,

    CONSTRAINT FK_District_Block_BlockId FOREIGN KEY (BlockId)  REFERENCES Block(Id),
    CONSTRAINT FK_District_Block_DistrictId FOREIGN KEY (DistrictId)  REFERENCES District(Id)
);

