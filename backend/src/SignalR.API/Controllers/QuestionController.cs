using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.API.Hubs;
using SignalR.API.Migrations;
using SignalR.API.Models;
using SignalR.API.Services;

namespace SignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IHubContext<QuestionHub, IQuestionHub> hubContext;
        private readonly IQuestionService service;
        public QuestionController(
            IQuestionService service,
            IHubContext<QuestionHub, IQuestionHub> questionHub)
        {
            this.service = service;
            this.hubContext = questionHub;
        }

        [HttpGet]
        public async Task<IEnumerable> GetAllAsync()
            => await service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<Question> GetAsync(Guid id)
            => await service.GetAsync(id);

        [HttpPost]
        public async Task<Question> AddQuestionAsync([FromBody] Question question)
            => await service.AddQuestionAsync(question);

        [HttpPost("{id}/answer")]
        public async Task<ActionResult> AddAnswerAsync(Guid id, [FromBody] Answer answer)
        {
            var result = await service.AddAnswerAsync(id, answer);
            await hubContext
                    .Clients
                    .Group(id.ToString())
                    .AnswerAdded(result);
            return Ok(result);
        }

        [HttpPatch("{id}/upvote")]
        public async Task<ActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = await service.UpvoteQuestionAsync(id);
            await hubContext
                    .Clients
                    .All
                    .QuestionScoreChange(question.Id, question.Score);
            return Ok(question);
        }

        [HttpPatch("{id}/downvote")]
        public async Task<ActionResult> DownvoteQuestionAsync(Guid id)
        {
            var question = await service.UpvoteQuestionAsync(id);
            await hubContext
                    .Clients
                    .All
                    .QuestionScoreChange(question.Id, question.Score);
            return Ok(question);
        }
    }
}