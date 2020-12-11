SELECT 

SUBSTRING(county.Id,1,2),
*

FROM county 

where SUBSTRING(county.Id,1,2) = '45'