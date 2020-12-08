
DROP TABLE IF EXISTS Block_Population_Time;
CREATE TABLE IF NOT EXISTS Block_Population_Time
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    Block_Id VARCHAR(50) NOT NULL, -- Primary Key column
    Reporting_Date DATE NOT NULL,
    Population INT NOT NULL
    -- Specify more columns here
);



DROP TABLE IF EXISTS Demographic;
CREATE TABLE IF NOT EXISTS Demographic
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    Population_Time_Id uuid NOT NULL,

    CONSTRAINT FK_PopulationTime_Demographic FOREIGN KEY (Population_Time_Id)  REFERENCES Block_Population_Time(Id)
);
