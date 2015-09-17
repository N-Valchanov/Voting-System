using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var submitVoteForm = GetPollByUrlId(id);

            return View(submitVoteForm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitVote(SubmitVoteInputModel input)
        {
            if (input == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (this.ModelState.IsValid)
            {
                var uidCookie = Request.Cookies["uid"];
                if (uidCookie == null)
                {
                    var uid = new HttpCookie("uid")
                    {
                        Expires = DateTime.Now.AddDays(30),
                        Value = Guid.NewGuid().ToString()
                    };
                    this.Response.Cookies.Add(uid);
                }
                else
                {
                    var s = uidCookie.Value;
                }

            }
            var submitVoteForm = GetPollByUrlId(input.QuestionUrlId);
            return this.View("~/Views/Vote/Index.cshtml", submitVoteForm);
        }
        private SubmitVoteInputModel GetPollByUrlId(string id)
        {
            var poll = from questions in db.Questions
                       join answers in db.Answers on questions.Id equals answers.QuestionId
                       where questions.UrlId == id
                       select questions;
            var submitVoteForm = new SubmitVoteInputModel();
            foreach (var question in poll.First().Answers)
            {
                submitVoteForm.Answers.Add(question.Content);
            }
            submitVoteForm.NamesRequired = poll.First().RequireNames;
            submitVoteForm.QuestionContent = poll.First().Content;
            submitVoteForm.QuestionUrlId = id;
            return submitVoteForm;
        }
    }
}