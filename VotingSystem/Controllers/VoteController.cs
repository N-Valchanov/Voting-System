using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;
using VotingSystem.Models.InputModels;

namespace VotingSystem.Controllers
{
    public class VoteController : Controller
    {

        private VotingSystemEntities db = new VotingSystemEntities();
        // GET: Vote
        public ActionResult Index(string id)
        {
            
            var poll = from questions in db.Questions join answers in db.Answers on questions.Id equals answers.QuestionId
                           where questions.UrlId == id 
                           select questions;
            var SubmitVoteForm = new SubmitVoteInputModel();
            foreach (var question in poll.First().Answers)
            {
                SubmitVoteForm.Answers.Add(question.Content);
            }
            SubmitVoteForm.NamesRequired = poll.First().RequireNames;
            SubmitVoteForm.Question = poll.First().Content;
            SubmitVoteForm.QuestionId = poll.First().Id;

            return View(SubmitVoteForm);
        }
    }
}