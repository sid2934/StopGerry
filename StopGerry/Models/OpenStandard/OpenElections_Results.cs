using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using StopGerry.Utilities;

namespace StopGerry.Models.OpenStandard
{
    public class OpenElections_Results
    {
        public string county { get; set; }
        public string precinct { get; set; }
        public string office { get; set; }
        public string district { get; set; }
        public string party { get; set; }
        public string candidate { get; set; }
        public string votes { get; set; }



        private OpenElections_Info _info;

        private County _linkedCounty;

        private int _linkedCandidateId;

        //? This can possibly removed depending on the Id used for districts
        private string _districtCode;


        private void LinkCandidateData(stopgerryContext dbContext)
        {

            //Look for a matching candidate, if one is not found create it
            var linkedCandidate = dbContext.Candidate.Where(c => c.Name == candidate && c.Party == party).FirstOrDefault();
            if (linkedCandidate == null)
            {
                linkedCandidate = new Candidate()
                {
                    Name = candidate,
                    Party = party,
                };
                SimpleLogger.Info($"Candidate was not found. Created new record {ObjectDumper.Dump(linkedCandidate)}");
                dbContext.Add(linkedCandidate);
                dbContext.SaveChanges(); //Changes need to be saved otherwise the foreign keys complain later
            }
            _linkedCandidateId = linkedCandidate.Id;

        }

        private void LinkLocationData()
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                _districtCode = null;
            }
            else
            {
                //First reconstruct the format for the district id
                //{stateFips}{District #} !! with leading zerons
                _districtCode = _info.State.Id.ToString("00") + Convert.ToInt32(Regex.Match(district, @"\d+").Value).ToString("000");

                if (_districtCode.Length != 5)
                {
                    SimpleLogger.Error($"While trying to find the linked district there was an error parsing the district code.\n\t Calculated District code <stateFips><District #> was {_districtCode}");
                }
            }
        }

        /// <summary>
        /// This method is used to find the Electionrace, Candidate, and District associcated with this result
        /// </summary>
        /// <returns></returns>
        private Tuple<int, string> GetMinimalTuple()
        {
            //We no nothing
            return new Tuple<int, string>(_linkedCandidateId, _districtCode);
        }


        public Tuple<int, string> SaveToDB(OpenElections_Info electionInfo, stopgerryContext dbContext, Tuple<int, string> shortcut = null)
        {
            _info = electionInfo;

            if (shortcut?.Item1 != null)
            {
                //we have what we need
                _linkedCandidateId = shortcut.Item1;
            }
            else
            {
                LinkCandidateData(dbContext);
            }

            if (shortcut?.Item2 != null)
            {
                _districtCode = shortcut.Item2;
            }
            else
            {
                LinkLocationData();
            }



            dbContext.Result.Add(new Result()
            {
                Id = Guid.NewGuid(),
                CountyId = _info.CountyDictionary[county.ToLower()],
                ResultResolution = _info.ResultsResolution,
                Precinct = _info.ResultsResolution == "precinct" ? precinct : null,
                Office = office,
                DistrictCode = _districtCode,
                CandidateId = _linkedCandidateId,
                NumberOfVotesRecieved = int.Parse(votes, NumberStyles.AllowThousands),
                ElectionDate = _info.ElectionDate,
                ElectionType = _info.ElectionType,
                Source = _info.Url,
            });

            return GetMinimalTuple();
            //Check if this election already exists.

        }
    }
}