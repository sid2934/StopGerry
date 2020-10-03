
DROP TABLE IF EXISTS Block_Population_Time;
CREATE TABLE IF NOT EXISTS Block_Population_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    BlockId VARCHAR(50) NOT NULL, -- Primary Key column
    ReportingDate DATE NOT NULL,
    Population INT NOT NULL
    -- Specify more columns here
);



DROP TABLE IF EXISTS Demographic;
CREATE TABLE IF NOT EXISTS Demographic
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    PopulationTimeId uuid NOT NULL,

    CONSTRAINT FK_PopulationTime_Demographic FOREIGN KEY (PopulationTimeId)  REFERENCES Block_Population_Time(Id)
);
