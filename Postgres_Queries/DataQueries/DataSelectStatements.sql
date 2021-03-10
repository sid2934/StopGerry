SELECT 

countyid AS LocationId,
er.id AS RaceId,
r.candidateid AS CandidateId,
c.partyid AS PartyId,
r.numberofvotesrecieved AS VotesRecieved


FROM result AS r
    LEFT OUTER JOIN electionrace AS er ON r.ElectionRaceId = er.id
    LEFT OUTER JOIN county_election AS ce ON er.countyelectionid = ce.id
    LEFT OUTER JOIN candidate AS c ON r.candidateid = c.id
