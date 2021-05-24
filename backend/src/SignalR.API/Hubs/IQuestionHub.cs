using SignalR.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public interface IQuestionHub
    {
        Task QuestionScoreChange(Guid questionId, int score);
        Task AnswerAdded(IEnumerable<Answer> answers);
    }
}