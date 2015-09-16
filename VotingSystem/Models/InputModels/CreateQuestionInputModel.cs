using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Models.InputModels
{
    public class CreateQuestionInputModel
    {
        public CreateQuestionInputModel()
        {
            this.Answers = new List<string>() { "","" ,"" };
        }

        [Display(Name = "Question:")]
        [Required(ErrorMessage = "Please enter a valid question")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Question content must be between 1 and 250 characters long.")]
        public string Content { get; set; }

        [Display(Name = "Require names?")]
        public bool RequireNames { get; set; }

        public List<string> Answers { get; set; }
    }
}