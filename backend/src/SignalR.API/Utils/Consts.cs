using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.API.Utils
{
    public static class Consts
    {
        public static IDictionary<string, IEnumerable<string>> QuestionGroups
            = new Dictionary<string, IEnumerable<string>>();

        public static IEnumerable<string> GetQuestionGroupConnectionIds(Guid questionId)
        {
            _ = QuestionGroups.TryGetValue(questionId.ToString(), out IEnumerable<string> result);
            return result;
        }

        public static IEnumerable<string> AddConnectionIdToQuestionGroups(Guid questionId, string connectionId)
        {
            if (QuestionGroups.ContainsKey(questionId.ToString()) &&
                !QuestionGroups[questionId.ToString()].Any(id => id == connectionId))
            {
                var data = QuestionGroups[questionId.ToString()].ToList();
                data.Add(connectionId);
                QuestionGroups[questionId.ToString()] = data;
            }

            if (!QuestionGroups.ContainsKey(questionId.ToString()))
            {
                QuestionGroups.Add(questionId.ToString(), new List<string>() { connectionId });
            }
            return GetQuestionGroupConnectionIds(questionId);
        }

        public static IEnumerable<string> RemoveConnectionIdFromQuestionGroups(Guid questionId, string connectionId)
        {
            if (QuestionGroups.ContainsKey(questionId.ToString()) &&
                QuestionGroups[questionId.ToString()].Any(id => id == connectionId))
            {
                var data = QuestionGroups[questionId.ToString()].ToList();
                data.Remove(connectionId);
                QuestionGroups[questionId.ToString()] = data;
            }
            return GetQuestionGroupConnectionIds(questionId);
        }
    }
}