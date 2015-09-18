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

        
        // GET: Vote
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var submitVoteForm = GetPollByUrlId(id);
            if (submitVoteForm.QuestionContent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
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
            if (string.IsNullOrWhiteSpace(input.QuestionUrlId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pollInfo = GetPollByUrlId(input.QuestionUrlId);
            if (pollInfo.QuestionContent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (pollInfo.NamesRequired)
            {
                ValidateName(input.FullName);
            }
            if (this.ModelState.IsValid)
            {
                var uidCookie = Request.Cookies["uid"];
                if (uidCookie == null)
                {
                    uidCookie = new HttpCookie("uid")
                    {
                        Expires = DateTime.Now.AddDays(30),
                        Value = Guid.NewGuid().ToString()
                    };
                    this.Response.Cookies.Add(uidCookie);
                }
                
                var db = new VotingSystemEntities();
                var cookieID = uidCookie.Value;
                var ip = Request.UserHostAddress.ToString();
                var checkIfVoted = from votes in db.Votes
                                   where votes.QuestionId== pollInfo.QuestionId && (votes.Ip == ip || votes.SecretKey == cookieID)
                                   select votes;
                if (checkIfVoted.Any())
                {
                    pollInfo.AlrdyVoted = true;
                    return View("~/Views/Vote/Index.cshtml", pollInfo);
                }
                var poll =from questions in db.Questions
                                      join answers in db.Answers on questions.Id equals answers.QuestionId
                                      where questions.UrlId == input.QuestionUrlId
                                      select questions;
                if (input.AnswerPicked>=poll.First().Answers.Count|| input.AnswerPicked<0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Vote v = new Vote();
                v.AnswerId = poll.First().Answers.ElementAt(input.AnswerPicked).Id; ;
                v.FullName = input.FullName;
                v.Ip = ip;
                v.QuestionId = poll.First().Id;
                v.SecretKey = cookieID;
                db.Votes.Add(v);
                db.SaveChanges();
             //To do: impliment result models   return Redirect("~View/Result/Index.cshtml",resultModel)

            }
            return View("~/Views/Vote/Index.cshtml", pollInfo);
        }
        private SubmitVoteInputModel GetPollByUrlId(string id)
        {
            var db = new VotingSystemEntities();
            var poll = from questions in db.Questions
                       join answers in db.Answers on questions.Id equals answers.QuestionId
                       where questions.UrlId == id
                       select questions;
            
            var submitVoteForm = new SubmitVoteInputModel();
            if (poll.Any())
            {
                foreach (var question in poll.First().Answers)
                {
                    submitVoteForm.Answers.Add(question.Content);
                }
                submitVoteForm.NamesRequired = poll.First().RequireNames;
                submitVoteForm.QuestionContent = poll.First().Content;
                submitVoteForm.QuestionId = poll.First().Id;
                submitVoteForm.QuestionUrlId = id;
            }
            return submitVoteForm;            
        }
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                this.ModelState.AddModelError("FullName", "Please add your name");
            }
        }
    }
}