using System;
using System.Collections;
using System.Threading.Tasks;
using SignalR.API.Models;

namespace SignalR.API.Services
{
    public interface IQuestionService
    {
        Task<Question> GetAsync(Guid id);
        Task<IEnumerable> GetAllAsync();
        Task<Question> AddQuestionAsync(Question question);
        Task<Answer> AddAnswerAsync(Guid id, Answer answer);
        Task<Question> UpvoteQuestionAsync(Guid id);
        Task<Question> DownvoteQuestionAsync(Guid id);
    }
}