using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class ResultHelperAnswers
    {
        public ResultHelperAnswers()
        {
            numVotes = 0;
        }
        public string answerContent { get; set; }
        public int numVotes { get; set; }
        public string percentageOftotalVotes { get; set; }

        public void PreparePercentage(int totalVotes)
        {
            if (totalVotes == 0 || numVotes == 0)
            {
                percentageOftotalVotes = "0% (0 out of "+totalVotes+" votes )";
            }
            else
            {
                percentageOftotalVotes = decimal.Divide(numVotes, totalVotes).ToString("P2")+"  ("+numVotes+" out of "+totalVotes+" votes )";
            }
            
        }
    }
}