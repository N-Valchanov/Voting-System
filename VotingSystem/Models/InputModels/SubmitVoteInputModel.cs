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
            AlrdyVoted = false;
        }
        public string QuestionUrlId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public int AnswerPicked { get; set; }
        public List<string> Answers { get; set; }
        public bool NamesRequired { get; set; }
        public bool AlrdyVoted { get; set; }

        [Display(Name = "Your Name:")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name can only be up to 50 characters long.")]
        public string FullName { get; set; }
    }
}