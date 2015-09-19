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
    }
}