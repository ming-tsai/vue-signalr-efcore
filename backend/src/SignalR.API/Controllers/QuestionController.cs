using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.API.Hubs;
using SignalR.API.Models;

namespace SignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private static IEnumerable<Question> questions = new List<Question> {
            new Question
            {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                Title = "Welcome",
                Body = "Welcome to the _mini Stack Overflow_ rip-off!\nThis will help showcasing **SignalR** and its integration with **Vue**",
                Answers = new List<Answer>{ new Answer { Body = "Sample answer" }}
            }
        };

        private readonly IHubContext<QuestionHub, IQuestionHub> hubContext;
        public QuestionController(IHubContext<QuestionHub, IQuestionHub> questionHub)
        {
            this.hubContext = questionHub;
        }

        [HttpGet]
        public IEnumerable GetQuestions()
        {
            return questions.Select(q => new
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                Score = q.Score,
                AnswerCount = q.Answers.Count()
            });
        }

        [HttpGet("{id}")]
        public Question GetQuestion(Guid id)
            => questions.FirstOrDefault(t => t.Id == id);

        [HttpPost]
        public Question AddQuestion([FromBody] Question question)
        {
            question.Id = Guid.NewGuid();
            question.Answers = new List<Answer>();
            questions.Append(question);
            return question;
        }

        [HttpPost("{id}/answer")]
        public async Task<ActionResult> AddAnswerAsync(Guid id, [FromBody] Answer answer)
        {
            var question = questions.FirstOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            answer.Id = Guid.NewGuid();
            answer.QuestionId = id;
            question.Answers.Append(answer);
            await hubContext
                    .Clients
                    .Group(id.ToString())
                    .AnswerAdded(answer);
            return Ok(answer);
        }

        [HttpPatch("{id}/upvote")]
        public async Task<ActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = questions.FirstOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            question.Score++;
            await hubContext
                    .Clients
                    .All
                    .QuestionScoreChange(question.Id, question.Score);
            return Ok(question);
        }
    }
}