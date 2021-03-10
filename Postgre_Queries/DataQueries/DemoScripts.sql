--Checks to see the current amount of results are in the database in total

/*Test State info

OHIO (https://planscore.org/ohio/#!2018-plan-ushouse-eg)
    State FIPS = 39
    U.S. House Seats = 16 
    State House Seats = 99
    State Senate Seats = 33



*/




SELECT Count(*) From result WHERE County_Id LIKE '39%'