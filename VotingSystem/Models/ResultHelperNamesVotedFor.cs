using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class ResultHelperNamesVotedFor
    {
        public string Name { get; set; }
        public string AnswerVotedFor { get; set; }
        public string PreparedNamedVotes { get { return Name + " has voted: " + AnswerVotedFor; } }

    }
}
