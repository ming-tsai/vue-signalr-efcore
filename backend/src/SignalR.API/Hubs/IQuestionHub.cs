using System;
using System.Threading.Tasks;
using SignalR.API.Models;

namespace SignalR.API.Hubs
{
    public interface IQuestionHub
    {
        Task QuestionScoreChange(Guid questionId, int score);
        Task AnswerAdded(Answer answer);
    }
}