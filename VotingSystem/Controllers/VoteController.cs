﻿using System;
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
                var alrdyVoted = false;
                var getQuestinoId = from questions in db.Questions
                                    where questions.UrlId == input.QuestionUrlId
                                    select questions.Id;
                int x = getQuestinoId.First();
                if (getQuestinoId== null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var checkIfVoted = from votes in db.Votes
                                   where votes.QuestionId== x && (votes.Ip == ip || votes.SecretKey == cookieID)
                                   select votes;
                if (checkIfVoted.Any())
                {
                    alrdyVoted = true;//To do: Validate Name, fix Result interactions
                }
                var poll =from questions in db.Questions
                                      join answers in db.Answers on questions.Id equals answers.QuestionId
                                      where questions.UrlId == input.QuestionUrlId
                                      select questions;
                if (input.AnswerPicked>=poll.First().Answers.Count|| input.AnswerPicked<0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var s = poll.First().Answers.ElementAt(input.AnswerPicked).Id;
                Vote v = new Vote();
                v.AnswerId = s;
                v.FullName = input.FullName;
                v.Ip = ip;
                v.QuestionId = poll.First().Id;
                v.SecretKey = cookieID;
                db.Votes.Add(v);
                db.SaveChanges();

            }
            var submitVoteForm = GetPollByUrlId(input.QuestionUrlId);
            return View("~/Views/Vote/Index.cshtml", submitVoteForm);
        }
        private SubmitVoteInputModel GetPollByUrlId(string id)
        {
            var db = new VotingSystemEntities();
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