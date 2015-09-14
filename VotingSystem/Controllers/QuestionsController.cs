using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using VotingSystem.Models;
using VotingSystem.Models.InputModels;

namespace VotingSystem.Controllers
{
    public class QuestionsController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateQuestionInputModel inputModel)
        {
            if (inputModel != null && this.ModelState.IsValid)
            {
                this.ValidateAnswers(inputModel.Answers);

                if (this.ModelState.IsValid)
                {
                    var context = new VotingSystemEntities();

                    var newQuestion = new Question { Content = inputModel.Content, RequireNames = inputModel.RequireNames };

                    this.AddQuestionAnswers(inputModel.Answers, newQuestion);

                    this.GenerateQuestionUrlId(newQuestion);

                    context.Questions.Add(newQuestion);
                    context.SaveChanges();

                    return this.Redirect("/" + newQuestion.UrlId);
                }
            }

            return this.View("~/Views/Home/Index.cshtml", inputModel ?? new CreateQuestionInputModel());
        }

        private void GenerateQuestionUrlId(Question newQuestion)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = 10;
            const int ByteSize = 0x100;
            const int BufferSize = 128;

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
            }

            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentException("allowedChars may not be empty.");
            }

            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (ByteSize < allowedCharSet.Length)
            {
                throw new ArgumentException($"allowedChars may contain no more than {ByteSize} characters.");
            }

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buffer = new byte[BufferSize];
                while (result.Length < length)
                {
                    rng.GetBytes(buffer);
                    for (var i = 0; i < buffer.Length && result.Length < length; i++)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = ByteSize - (ByteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buffer[i])
                        {
                            continue;
                        }

                        result.Append(allowedCharSet[buffer[i] % allowedCharSet.Length]);
                    }
                }

                newQuestion.UrlId = result.ToString();
            }
        }

        private void AddQuestionAnswers(List<string> answers, Question question)
        {
            foreach (var answer in answers)
            {
                if (!string.IsNullOrWhiteSpace(answer))
                {
                    question.Answers.Add(new Answer { Content = answer });
                }
            }
        }

        private void ValidateAnswers(List<string> answers)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].Length > 200)
                {
                    this.ModelState.AddModelError("Answers[" + i + "]", "Answer #" + (i + 1) + " must be max 200 characters long.");
                }
            }

            if (answers.Count(x => !string.IsNullOrWhiteSpace(x)) < 2)
            {
                this.ModelState.AddModelError(string.Empty, "You must fill at least two answers.");
            }
        }
    }
}