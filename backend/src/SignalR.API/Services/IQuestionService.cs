using SignalR.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.API.Services
{
    public interface IQuestionService
    {
        Task<Question> GetAsync(Guid id);
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> AddQuestionAsync(Question question);
        Task<Answer> AddAnswerAsync(Guid id, Answer answer);
        Task<Question> UpvoteQuestionAsync(Guid id);
        Task<Question> DownvoteQuestionAsync(Guid id);
    }
}