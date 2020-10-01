/* DROP ALL TABLES
DROP SCHEMA "Census" CASCADE;
*/

CREATE SCHEMA IF NOT EXISTS "Census";

DROP TABLE IF EXISTS "Census".Block_Population_Time;
CREATE TABLE IF NOT EXISTS "Census"."Block_Population_Time"
(
    "Id" uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(),
    "BlockId" VARCHAR(50) NOT NULL, -- Primary Key column
    "ReportingDate" DATE NOT NULL,
    "Population" INT NOT NULL
    -- Specify more columns here
);



DROP TABLE IF EXISTS "Census".Demographic;
CREATE TABLE IF NOT EXISTS "Census"."Demographic"
(
    "Id" uuid PRIMARY KEY DEFAULT UUID_GENERATE_V4(), -- Primary Key column
    "PopulationTimeId" uuid NOT NULL,

    CONSTRAINT "FK_PopulationTime_Demographic" FOREIGN KEY ("PopulationTimeId")  REFERENCES "Census"."Block_Population_Time"("Id")
);
