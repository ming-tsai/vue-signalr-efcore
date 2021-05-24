using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SignalR.API.Hubs;
using SignalR.API.Migrations;
using SignalR.API.Utils;
using System;
using System.Threading.Tasks;

namespace SignalR.API.Services
{
    public class SqlDependencyNotification : ISqlDependencyNotification
    {
        private readonly IHubContext<QuestionHub, IQuestionHub> hubContext;
        private readonly IConfiguration configuration;

        public SqlDependencyNotification(
            IHubContext<QuestionHub, IQuestionHub> hubContext,
            IConfiguration configuration)
        {
            this.hubContext = hubContext;
            this.configuration = configuration;
        }

        public async Task SubscribeQuestionAsync(Guid questionId)
        {
            using var dbContext = GetDbcontextInstance();
            using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await SubscribeQuestionAnswerAsync(questionId, connection);
        }

        private async Task SubscribeQuestionAnswerAsync(Guid questionId, System.Data.Common.DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT Id FROM Answers WHERE QuestionId = '{questionId.ToString()}';";
            var scoreDependency = new SqlDependency((SqlCommand)command);
            scoreDependency.OnChange += OnAnswerAdded;
            SqlDependency.Start(configuration.GetConnectionString("SqlServer"));
            _ = await command.ExecuteReaderAsync();
        }

        private void OnAnswerAdded(object sender, SqlNotificationEventArgs e)
        {
            using var dbContext = GetDbcontextInstance();
            var service = new QuestionService(dbContext);
            var groups = Consts.QuestionGroups.Keys;
            foreach (var group in groups)
            {
                var question = Task.Run(async () => await service.GetAsync(Guid.Parse(group))).Result;
                _ = Task.Run(async () => await SubscribeQuestionAsync(question.Id));
                _ = Task.Run(async () =>
                        await hubContext
                                .Clients
                                .Group(group)
                                .AnswerAdded(question.Answers)
                );
            }
        }

        private ApplicationDbContext GetDbcontextInstance()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseSqlServer(configuration.GetConnectionString("SqlServer"))
                            .Options;
            return new ApplicationDbContext(options);
        }
    }
}