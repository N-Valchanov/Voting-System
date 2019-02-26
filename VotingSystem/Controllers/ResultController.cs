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
        [HttpGet]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resultModel = GetResultModel(id);
            if (resultModel.QuestionContent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(resultModel);
        }

        private ResultModel GetResultModel(string id)
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
            resultModel.TotalVotes = poll.First().Votes.Count;
            foreach (var answer in poll.First().Answers)
            {
                resultModel.Answers.Add(new ResultHelperAnswers { answerContent = answer.Content, numVotes = answer.Votes.Count() });
                resultModel.Answers.Last().PreparePercentage(resultModel.TotalVotes);
            }
            if (resultModel.NamesRequired && poll.First().Votes.Any())
            {
                foreach (var vote in poll.First().Votes)
                {
                    resultModel.NamedVotes.Add(new ResultHelperNamesVotedFor { Name = vote.FullName, AnswerVotedFor = vote.Answer.Content });
                }
            }
            resultModel.Answers.Sort((x, y) => y.numVotes.CompareTo(x.numVotes));
            return resultModel;
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