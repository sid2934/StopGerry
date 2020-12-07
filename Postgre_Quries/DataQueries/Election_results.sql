SELECT DISTINCT

ce.id AS CountyElectionId,
c.id AS CountyFIPS,
c.description AS CountyName,
ert.Description,
ca.id AS CandidateId,
ca."Name" as CandidateName,
p."Name",
SUM(r.numberofvotesrecieved) as VotesRecieved

FROM result AS r
    LEFT OUTER JOIN electionrace AS er ON r.electionraceid = er.id
    LEFT OUTER JOIN electionrace_type AS ert ON er.Electionracetypeid = ert.id
    LEFT OUTER JOIN candidate AS ca ON r.candidateid = ca.id
    LEFT OUTER JOIN party AS p ON ca.partyid = p.id
    LEFT OUTER JOIN county_election as ce ON er.countyelectionid = ce.id
    LEFT OUTER JOIN county as c on ce.countyid = c.id

WHERE ert.Description = 'President'

GROUP BY ce.id, c.id, c.description, ca.id, ca."Name", ert.Description, p."Name"
