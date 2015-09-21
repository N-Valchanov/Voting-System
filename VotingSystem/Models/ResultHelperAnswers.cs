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
        public string rawPercentage { get; set; }
        public string PercentageForm { get; set; }

        public void PreparePercentage(int totalVotes)
        {
            string attachmentToPercent;
            if (totalVotes == 1)
            {
                attachmentToPercent = totalVotes + " vote )";
            }
            else
            {
                attachmentToPercent = totalVotes + " votes )";
            }
            if (totalVotes == 0 || numVotes == 0)
            {
                rawPercentage = "0%";
                PercentageForm =rawPercentage + " ( 0 out of " + attachmentToPercent;
            }
            else
            {
                rawPercentage = decimal.Divide(numVotes, totalVotes).ToString("P2");
                rawPercentage= rawPercentage.Replace(" ", string.Empty);
                PercentageForm =rawPercentage + "  ( " + numVotes + " out of " + attachmentToPercent;
            }
            
        }
    }
}