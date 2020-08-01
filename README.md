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

The goal of StopGerry is to provide researchers an accessable tool kit to study the effect of gerrymandering
