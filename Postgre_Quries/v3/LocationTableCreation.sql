/*
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
ALTER SCHEMA public OWNER TO stopgerry;
GRANT USAGE ON SCHEMA public TO cis625;
GRANT USAGE ON ALL TABLES IN SCHEMA public TO cis625;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO cis625;

*/

DROP TABLE IF EXISTS Block;
CREATE TABLE IF NOT EXISTS Block
(
 Id          VARCHAR(50) NOT NULL PRIMARY KEY,
 Description VARCHAR(255) NOT NULL,
 Source      VARCHAR(255) NOT NULL,
 Coordinates GEOMETRY NOT NULL,
 Border      GEOMETRY NULL

);

DROP TABLE IF EXISTS County;
CREATE TABLE IF NOT EXISTS County
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    Description VARCHAR(255) NOT NULL,
    State_Id INT NOT NULL,
    Source TEXT NOT NULL,
    Border GEOMETRY NULL

);


DROP TABLE IF EXISTS State;
CREATE TABLE IF NOT EXISTS State
(
    Id INT NOT NULL PRIMARY KEY, -- Primary Key column
    Name VARCHAR(50) NOT NULL,
    Abbreviation VARCHAR(2) NOT NULL,
    Source TEXT NOT NULL,
    State_Type VARCHAR(20) NOT NULL,
    County_Type VARCHAR(20) NOT NULL,
    Border GEOMETRY NULL
    
);

DROP TABLE IF EXISTS State_Time;
CREATE TABLE IF NOT EXISTS State_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    State_Id INT NOT NULL, -- Primary Key column
    Time_Start DATE NOT NULL,
    Time_End DATE NULL,
    

    CONSTRAINT FK_State_Time_State FOREIGN KEY (State_Id)  REFERENCES State(Id)
);

DROP TABLE IF EXISTS County_Time;
CREATE TABLE IF NOT EXISTS County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    County_Id VARCHAR(50) NOT NULL, -- Primary Key column
    Time_Start DATE NOT NULL,
    Time_End DATE NULL,
    State_Id INT NOT NULL,

    CONSTRAINT FK_State_County_County_Id FOREIGN KEY (County_Id)  REFERENCES County(Id),
    CONSTRAINT FK_State_County_State_Id FOREIGN KEY (State_Id)  REFERENCES State(Id)
);

DROP TABLE IF EXISTS Block_County_Time;
CREATE TABLE IF NOT EXISTS Block_County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    Block_Id VARCHAR(50) NOT NULL, -- Primary Key column
    Time_Start DATE NOT NULL,
    Time_End DATE NULL,
    County_Id VARCHAR(50) NOT NULL,

    CONSTRAINT FK_County_Block_Block_Id FOREIGN KEY (Block_Id)  REFERENCES Block(Id),
    CONSTRAINT FK_County_Block_County_Id FOREIGN KEY (County_Id)  REFERENCES County(Id)
);

DROP TABLE IF EXISTS District;
CREATE TABLE IF NOT EXISTS District
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    Year INT NOT NULL,
    District_Code VARCHAR(6) NOT NULL,
    Description TEXT NOT NULL,
    Source TEXT NOT NULL,
    District_Type VARCHAR(50) NOT NULL,
    Border geometry NULL

);

DROP TABLE IF EXISTS District_Time;
CREATE TABLE IF NOT EXISTS District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    District_Id VARCHAR(50) NOT NULL,
    Time_Start DATE NOT NULL,
    Time_End DATE NULL

);


DROP TABLE IF EXISTS Block_District_Time;
CREATE TABLE IF NOT EXISTS Block_District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    Block_Id VARCHAR(50) NOT NULL, -- Primary Key column
    Time_Start DATE NOT NULL,
    Time_End DATE NULL,
    District_Id uuid NOT NULL,

    CONSTRAINT FK_District_Block_Block_Id FOREIGN KEY (Block_Id)  REFERENCES Block(Id),
    CONSTRAINT FK_District_Block_District_Id FOREIGN KEY (District_Id)  REFERENCES District(Id)
);

