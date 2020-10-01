CREATE TABLE "Location"."Block"
(
 Id          VARCHAR(50) NOT NULL PRIMARY KEY,
 Description VARCHAR(255) NOT NULL ,
 Source      VARCHAR(255) NOT NULL ,
 Coordinates geometry NOT NULL ,
 Border      geometry NULL 
);