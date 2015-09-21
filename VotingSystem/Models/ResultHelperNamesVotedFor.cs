using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class ResultHelperNamesVotedFor
    {
        public string name { get; set; }
        public string answerVotedFor { get; set; }

        public string PrepareNamedVotes()
        {
            return name + " has voted: " + answerVotedFor;
        }
    }
}