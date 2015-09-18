using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Models.InputModels
{
    public class SubmitVoteInputModel
    {
        public SubmitVoteInputModel()
        {
            this.Answers = new List<string>();
        }
        public string QuestionUrlId { get; set; }
        public string QuestionContent { get; set; }
        public int AnswerPicked { get; set; }
        public List<string> Answers { get; set; }
        public bool NamesRequired { get; set; }

        [Display(Name = "Your Name:")]
        public string FullName { get; set; }
    }
}