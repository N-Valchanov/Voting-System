using System.Web.Mvc;
using VotingSystem.Models.InputModels;

namespace VotingSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new CreateQuestionInputModel());
        }
    }
}