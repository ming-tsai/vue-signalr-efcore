using Microsoft.AspNetCore.SignalR;
using SignalR.API.Services;
using SignalR.API.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class QuestionHub : Hub<IQuestionHub>
    {
        private readonly ISqlDependencyNotification dependencyNotification;
        public QuestionHub(ISqlDependencyNotification dependencyNotification)
        {
            this.dependencyNotification = dependencyNotification;
        }

        public async Task JoinQuestionGroup(Guid questionId)
        {
            var connectionIds = Consts.AddConnectionIdToQuestionGroups(questionId, Context.ConnectionId);
            if (connectionIds.Count() <= 1)
            {
                await dependencyNotification.SubscribeQuestionAsync(questionId);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, questionId.ToString());
        }

        public async Task LeaveQuestionGroup(Guid questionId)
        {
            Consts.RemoveConnectionIdFromQuestionGroups(questionId, Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, questionId.ToString());
        }
    }
}