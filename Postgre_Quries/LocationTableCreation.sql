/* DROP ALL TABLES
DROP SCHEMA "Location" CASCADE;
*/

CREATE SCHEMA IF NOT EXISTS "Location";

DROP TABLE IF EXISTS "Location".Block;
CREATE TABLE IF NOT EXISTS "Location".Block
(
 Id          VARCHAR(50) NOT NULL PRIMARY KEY,
 Description VARCHAR(255) NOT NULL,
 Source      VARCHAR(255) NOT NULL,
 Coordinates geometry NOT NULL,
 Border      geometry NULL

);


-- CountyType Start
DROP TABLE IF EXISTS "Location".CountyType;
CREATE TABLE IF NOT EXISTS "Location".CountyType
(
    Id SERIAL PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

INSERT INTO "Location".CountyType
( -- Columns to insert data into
 Description
)
VALUES
( -- First row: values for the columns in the list above
 'County'
),
( -- Second row: values for the columns in the list above
 'Borough'
),
(
 'Parish'
),
(
 'County Equivalent'
);
-- CountyType End

DROP TABLE IF EXISTS "Location".County;
CREATE TABLE IF NOT EXISTS "Location".County
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    Description VARCHAR(255) NOT NULL,
    Source text NOT NULL,
    Border geometry NULL

);

-- StateType Start 
DROP TABLE IF EXISTS "Location".StateType;
CREATE TABLE IF NOT EXISTS "Location".StateType
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL
);

INSERT INTO "Location".StateType
( -- Columns to insert data into
 Description
)
VALUES
( -- First row: values for the columns in the list above
 'State'
),
( -- Second row: values for the columns in the list above
 'Outlying area under U.S. sovereignty'
),
(
 'Minor outlying island territory'
),
(
    'Federal district'
),
(
    'Freely Associated State'
);

-- StateType End


DROP TABLE IF EXISTS "Location".State;
CREATE TABLE IF NOT EXISTS "Location".State
(
    Id INT NOT NULL PRIMARY KEY, -- Primary Key column
    Name VARCHAR(50) NOT NULL,
    Abbreviation VARCHAR(2) NOT NULL,
    Source text NOT NULL,
    StateTypeId INT NOT NULL,
    CountyTypeId INT NOT NULL,
    Border GEOMETRY NULL,
    
    CONSTRAINT FK_CountyType_State FOREIGN KEY (CountyTypeId)  REFERENCES "Location".CountyType(Id),
    CONSTRAINT FK_StateType_State FOREIGN KEY (StateTypeId)  REFERENCES "Location".StateType(Id)
);

DROP TABLE IF EXISTS "Location".State_Time;
CREATE TABLE IF NOT EXISTS "Location".State_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    StateId INT NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    

    CONSTRAINT FK_StateTime_State FOREIGN KEY (StateId)  REFERENCES "Location".State(Id)
);

DROP TABLE IF EXISTS "Location".County_Time;
CREATE TABLE IF NOT EXISTS "Location".County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    CountyId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    StateId INT NOT NULL,

    CONSTRAINT FK_State_County_CountyId FOREIGN KEY (CountyId)  REFERENCES "Location".County(Id),
    CONSTRAINT FK_State_County_StateId FOREIGN KEY (StateId)  REFERENCES "Location".State(Id)
);

DROP TABLE IF EXISTS "Location".Block_County_Time;
CREATE TABLE IF NOT EXISTS "Location".Block_County_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    BlockId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    CountyId VARCHAR(50) NOT NULL,

    CONSTRAINT FK_County_Block_BlockId FOREIGN KEY (BlockId)  REFERENCES "Location".Block(Id),
    CONSTRAINT FK_County_Block_CountyId FOREIGN KEY (CountyId)  REFERENCES "Location".County(Id)
);

-- DistrictType Start
DROP TABLE IF EXISTS "Location".DistrictType;
CREATE TABLE IF NOT EXISTS "Location".DistrictType
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL,
    District_Type_Code VARCHAR(5) NOT NULL UNIQUE
);

INSERT INTO "Location".DistrictType
( -- Columns to insert data into
 Description,District_Type_Code
)
VALUES
( -- First row: values for the columns in the list above
 'Congressional','CD'
),
( -- Second row: values for the columns in the list above
 'State Lower House','SLDL'
),
(
 'State Upper House','SLDU'
);
-- DistrictType End

DROP TABLE IF EXISTS "Location".District;
CREATE TABLE IF NOT EXISTS "Location".District
(
    Id VARCHAR(50) NOT NULL PRIMARY KEY,
    Description text NOT NULL,
    Source text NOT NULL,
    DistrictTypeId INT NOT NULL,
    Border geometry NULL,


    CONSTRAINT FK_DistrictType_District FOREIGN KEY (DistrictTypeId)  REFERENCES "Location".DistrictType(Id)
);

DROP TABLE IF EXISTS "Location".District_Time;
CREATE TABLE IF NOT EXISTS "Location".District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    DistrictId VARCHAR(50) NOT NULL,
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,

    CONSTRAINT FK_DistrictTime_District FOREIGN KEY (DistrictId)  REFERENCES "Location".District(Id)
);


DROP TABLE IF EXISTS "Location".District_Time;
CREATE TABLE IF NOT EXISTS "Location".Block_District_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    BlockId VARCHAR(50) NOT NULL, -- Primary Key column
    TimeStart DATE NOT NULL,
    TimeEnd DATE NULL,
    DistrictId VARCHAR(50) NOT NULL,

    CONSTRAINT FK_District_Block_BlockId FOREIGN KEY (BlockId)  REFERENCES "Location".Block(Id),
    CONSTRAINT FK_District_Block_DistrictId FOREIGN KEY (DistrictId)  REFERENCES "Location".District(Id)
);

