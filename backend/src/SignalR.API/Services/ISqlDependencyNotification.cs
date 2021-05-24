using System;
using System.Threading.Tasks;

namespace SignalR.API.Services
{
    public interface ISqlDependencyNotification
    {
        Task SubscribeQuestionAsync(Guid questionId);
    }
}