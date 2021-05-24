using Microsoft.EntityFrameworkCore;
using SignalR.API.Migrations;
using SignalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext dbContext;
        public QuestionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Answer> AddAnswerAsync(Guid id, Answer answer)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(t => t.Id == id);
            Answer result = null;
            if (question != null)
            {
                answer.Id = Guid.NewGuid();
                answer.QuestionId = id;
                _ = await dbContext.Answers.AddAsync(answer);
                _ = await dbContext.SaveChangesAsync();
                result = answer;
            }
            return result;
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            question.Id = Guid.NewGuid();
            _ = await dbContext.Questions.AddAsync(question);
            _ = await dbContext.SaveChangesAsync();
            return question;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            IEnumerable<Question> result = null;
            var query = dbContext.Questions.Include((x) => x.Answers);
            if (await query.AnyAsync())
            {
                var data = await query.ToListAsync();
                result = data.Select((q) => new Question()
                {
                    Id = q.Id,
                    Body = q.Body,
                    Score = q.Score,
                    Title = q.Title,
                    Answers = q.Answers.Select((a) => new Answer()
                    {
                        Id = a.Id,
                        Body = a.Body
                    })
                });
            }
            return result;
        }

        public async Task<Question> GetAsync(Guid id)
        {
            return await dbContext
                            .Questions
                            .Include(q => q.Answers)
                            .Select((q) => new Question()
                            {
                                Id = q.Id,
                                Body = q.Body,
                                Score = q.Score,
                                Title = q.Title,
                                Answers = q.Answers.Select((a) => new Answer()
                                {
                                    Id = a.Id,
                                    Body = a.Body
                                })
                            })
                            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Question> UpvoteQuestionAsync(Guid id)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(t => t.Id == id);
            if (question != null)
            {
                // Warning, this increment isnt thread-safe! Use Interlocked methods
                question.Score++;
                _ = await dbContext.SaveChangesAsync();
            }

            return question;
        }

        public async Task<Question> DownvoteQuestionAsync(Guid id)
        {
            var question = await dbContext.Questions.FirstOrDefaultAsync(t => t.Id == id);
            if (question != null)
            {
                // Warning, this increment isnt thread-safe! Use Interlocked methods
                question.Score--;
                _ = await dbContext.SaveChangesAsync();
            }

            return question;
        }
    }
}