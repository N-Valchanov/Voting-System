using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class ResultModel
    {
        public ResultModel()
        {
            this.Answers = new List<ResultHelperAnswers>();
            this.NamedVotes = new List<ResultHelperNamesVotedFor>();
        }
        public List<ResultHelperAnswers> Answers { get; set; }
        public List<ResultHelperNamesVotedFor> NamedVotes { get; set; }
        public string QuestionUrlId { get; set; }
        public string QuestionContent { get; set; }
        public bool NamesRequired { get; set; }
    }
}