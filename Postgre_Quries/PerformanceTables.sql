
DROP TABLE IF EXISTS Performance_Analysis;
CREATE TABLE IF NOT EXISTS Performance_Analysis
(

    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4() NOT NULL,
    TotalRuntime BIGINT NOT NULL,
    NumberOfCoresAvailable INT NOT NULL,
    NumberOfBlocks INT NOT NULL,
    NumberOfDistricts INT NOT NULL,
    States TEXT NOT NULL,
    MemoryUsed BIGINT NOT NULL,
    HostName VARCHAR(60) NOT NULL,
    SystemPageSize INT NOT NULL,
    Jobid VARCHAR(255) NULL
);
