# StopGerry - is a project to create tools that can analyse voter data and district maps to determine if the maps are created in bad faith for political gain

## Current Status
The current status of the project is tin the initial development stage. We are activly developing the database infrastructure and data ingest tools that will eventually help drive the main application.

### Database_Files
This project subdirectory contains the files to create and schemes, tables, etc for the MSSQL Database. The main files are .ipynb Junyter notebooks. I use Azure Data Studio to work on them, which has support to view and edit them. They are seperated based on the schema they modify with the main 3 being Location, Census, annd Election.

* Location
  * Location store data related to the physical location of places and their relationship to larger orginizational units(OU). 
  * The base OU for StopGerry is the [Census Block](https://www.census.gov/newsroom/blogs/random-samplings/2011/07/what-are-census-blocks.html). This gives us a way to structure all of the data and link it to a single, realitivyly unchanging physical location.
  * The most difficult aspect of this project so far has been trying to find a way to track the physical locations over time since the borders of the smaller OUs change over time.


* Census
  * The Census schema is used to organize and store data related to U.S. Census Bureau collections.
  * The Census Bureau releases very detailed documents on how to use their data which has been very helpful.
  * We do not currently track much beyond the population per block, but in the future these tools can be expanded to include demographic data, economic data, etc.
  * All of the Census data is currently tied to a Location.Block. 

* Election
  * The Election schema is used to organize and store data related to election results.
  * Much of the available election data for public use is tied to the state, county, or precinct levels. This does not tie directly to the census block of Location.Block so we plan to connect this to the Location.County table instead.
  * In the future there may be a way to use the more specific data, but that will be an inprovment down the line

### Project Goals

The goal of StopGerry is to provide researchers an accessable tool kit to study the effect of gerrymandering. Ideally the project will grow in capability and expand the political aspects that it can analyse. 

### Challenges this project faces

During the inital creating of this project there were many unknowns and challenges that I could not have predicted.
1) The first problem that I found was the knowledge needed to use the publically availiable datasets and the ability to find a way to organize it all for this projects goals.
2) While developing this software I would test the underlying tooling on a single state and the runtime for data ingest and the size of the data were much larger than I would have predicted. Initally a single start would take nearly an hour to process for a single census report and the data was nearly 3GB.  
3) There is a large difference on how each state releases their election results. Importing election results is less of a standard import since each needs to be either designed to import from its raw format or pre processed into a general form that is then imported.
   
#### Acknowledgement and Disclaimer
I believe that it is important to disclose where much of this data has come from and to give some insite into the current state of the application.

This application is not in a state that I would reccomend anyone use it as is. There is not much available currently beyone the data ingest tooling, but if you choose to use this tool please take a careful look at the source code to ensure that it does what you expect.

All of the data used in this project comes directly from publically available sources. Some of note are
* The United States Census Bureau
* Each State'1s Secretart of State Website. A list can be found [here](https://www.thebalancesmb.com/secretary-of-state-websites-1201005) 


#### Things that need to be changed
1) The ability to process fixed width headers from the Census Bureau would save some manual pre-processing 
2) Add command line arguments to process individual files without modifying the resourceMap.csv file
3) Create a standard csv format for election data and census data to allow a user to import arbitrarily sourced data






#### Preprocessing census data

To preprocess the geoheader data for each state follow the directions below

1) Download the correct summary file 1 archive from https://www2.census.gov/census_2010/04-Summary_File_1/
2) Download the [Microsoft Access](https://www2.census.gov/census_2010/04-Summary_File_1/SF1_Access2007.accdb) file
3) Open the Access file
4) Follow [this guide](https://www2.census.gov/census_2010/04-Summary_File_1/0HowToUseMSAccessWithSummaryFile1.pdf) starting at step G. 
5) Export the new $$geo2010 file as csv with headers
6 Optional) Place the new .csv file in the resources/csv/ directory of the project
1) Add the new .csv file to the resourceMap.csv file