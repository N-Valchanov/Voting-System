using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class ResultController : Controller
    {
        private VotingSystemEntities db = new VotingSystemEntities();
        // GET: Result
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResultModel resultModel = new ResultModel();
            var resultFormHelper = GetVotesById(id);
            if (!resultFormHelper.Any())
            {
                resultModel = GetNamelessResultModel(id);
                if (resultModel.QuestionContent == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                return View(resultModel);
            }
            resultModel = GetNamelessResultModel(id);
            if (resultModel.NamesRequired)
            {
                foreach (var votes in resultFormHelper)
                {
                    resultModel.NamedVotes.Add(new ResultHelperNamesVotedFor { name = votes.FullName, answerVotedForId = votes.Answer.Content });
                }
            }
            return View(resultModel);
        }

        private ResultModel GetNamelessResultModel(string id)
        {
            ResultModel resultModel = new ResultModel();
            var poll = GetPollById(id);            
            if (!poll.Any())
            {
                return resultModel;
            }
            resultModel.QuestionContent = poll.First().Content;
            resultModel.QuestionUrlId = poll.First().UrlId;
            resultModel.NamesRequired = poll.First().RequireNames;
            foreach (var answer in poll.First().Answers)
            {
                resultModel.Answers.Add(new ResultHelperAnswers { answerContent = answer.Content, numVotes = answer.Votes.Count() });
            }
            return resultModel;
        }

        private IQueryable<Vote> GetVotesById (string id)
        {
            var vote =   from votes in db.Votes
                           join answer in db.Answers on votes.AnswerId equals answer.Id
                           join question in db.Questions on votes.QuestionId equals question.Id
                           where question.UrlId == id && votes.QuestionId == question.Id
                           orderby votes.AnswerId ascending
                           select votes;
            return vote;
        }

        private IQueryable<Question> GetPollById(string id)
        {
            var poll = from questions in db.Questions
                       where questions.UrlId == id
                       select questions;
            return poll;
        }
    }
}