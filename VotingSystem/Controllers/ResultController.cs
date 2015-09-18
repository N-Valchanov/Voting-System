using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace VotingSystem.Controllers
{
    public class ResultController : Controller
    {
        // GET: Result
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resultForm = GetPollResultsByUrlId(id);
            //if (submitVoteForm.QuestionContent == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            //}
            return View();
        }

        private object GetPollResultsByUrlId(string id)
        {
            throw new NotImplementedException();
        }
    }
}