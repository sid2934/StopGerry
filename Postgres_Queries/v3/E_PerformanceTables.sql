
DROP TABLE IF EXISTS Performance_Analysis;
CREATE TABLE IF NOT EXISTS Performance_Analysis
(

    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4() NOT NULL,
    Total_Runtime BIGINT NOT NULL,
    Number_Of_Cores_Available INT NOT NULL,
    Number_Of_Blocks INT NOT NULL,
    Number_Of_Districts INT NOT NULL,
    States TEXT NOT NULL,
    Memory_Used BIGINT NOT NULL,
    Hostname VARCHAR(60) NOT NULL,
    System_Page_Size INT NOT NULL,
    Job_Id VARCHAR(255) NULL
);
