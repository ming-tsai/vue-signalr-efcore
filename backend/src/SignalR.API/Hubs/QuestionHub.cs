using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.API.Hubs
{
    public class QuestionHub : Hub<IQuestionHub>
    {
        public async Task JoinQuestionGroup(Guid questionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, questionId.ToString());
        }
        public async Task LeaveQuestionGroup(Guid questionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, questionId.ToString());
        }
    }
}