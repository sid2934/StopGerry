# Version 1 Goals

The goal is to resurrect (the spirit of) this project and create something that is actually usable for end users.

## Facts

* There is a massive amount of publicly available data from elections, census, and other sources
* This data is useful on its own, but when combined with other information and proper analysis it may be even more important
* This project currently does not offer any real way to integrate data analysis
* The data storage for this project is EXTREMELY important because having an efficient storage pattern it will reduce the runtime for complex operations.
* But having really neat data is useless if you cannot do anything with it.

## Goals

### Stage 1

* Properly produce results about how gerrymandered districts are.
* Ensure that the gerrymandered calculations can be done in multiple ways.

### Stage 2

* Integrate the demographic data into the data set
  * This will require matching the demographic data to the voting resolution (precinct or county)
* Using demographic data determine if the gerrymandering is targeting specific groups
  * Race
  * Religion
  * Etc

### Stage 3

Stage 3 may seem like its coming in later than it should, but we need to get functionality
before we turn back and look for optimizations

* Improve the database schema and layouts.
* Introduce new dataset or components that come up in research
* StopGerry is a CLI tool, but should we generalize it into a SDK/Library? 