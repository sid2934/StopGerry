
/* THIS IS NO LONGER USED
DROP TABLE IF EXISTS County_Election;
CREATE TABLE IF NOT EXISTS County_Election
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    County_Id VARCHAR(50) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    Election_Date DATE NOT NULL,
    Election_Type VARCHAR(50) NOT NULL,

    CONSTRAINT FK_County_County_Election FOREIGN KEY (County_Id)  REFERENCES County(Id)
);
*/


DROP TABLE IF EXISTS Office;
CREATE TABLE IF NOT EXISTS Office
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Description VARCHAR(50) NOT NULL,
    Position_Level VARCHAR(50) NOT NULL

);


-- Insert rows into table 'Election_Race_Type' in schema 'Election'
INSERT INTO Office
( -- Columns to insert data into
    Description, Position_Level
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

DROP TABLE IF EXISTS Candidate;
CREATE TABLE IF NOT EXISTS Candidate
(
    Id SERIAL PRIMARY KEY, -- Primary Key column
    Name VARCHAR(50) NOT NULL,
    Party VARCHAR(50) NOT NULL,
    Date_Of_Birth DATE NOT NULL

);



CREATE TABLE IF NOT EXISTS Result
(
    Id uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    County_Id VARCHAR(50) NOT NULL,
    Precinct VARCHAR(255) NOT NULL,
    Office_Id INT NOT NULL,
    District_Code VARCHAR(6) NULL,
    Candidate_Id INT NOT NULL,
    Number_Of_Votes_Recieved INT NOT NULL,
    Source VARCHAR(255) NOT NULL,

    CONSTRAINT FK_County_Result FOREIGN KEY (County_Id) REFERENCES County(Id),
    CONSTRAINT FK_Office_Result FOREIGN KEY (Office_Id) REFERENCES Office(Id),
    CONSTRAINT FK_Candidate_Result FOREIGN KEY (Candidate_Id) REFERENCES Candidate(Id)

);