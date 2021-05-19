import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
export default {
  install(Vue) {
    // use new Vue instance as an event bus
    const questionHub = new Vue();
    // every component will use this.$questionHub to access the event bus
    Vue.prototype.$questionHub = questionHub;
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/question-hub')
      .configureLogging(LogLevel.Information)
      .build();
    let startedPromise = null;
    function start() {
      startedPromise = connection.start().catch((err) => {
        console.error('Failed to connect with hub', err);
        return new Promise((resolve, reject) =>
          setTimeout(() => start().then(resolve).catch(reject), 5000)
        );
      });
      return startedPromise;
    }
    questionHub.questionOpened = (questionId) => {
      return startedPromise
        .then(() => connection.invoke('JoinQuestionGroup', questionId))
        .catch(console.error);
    };
    questionHub.questionClosed = (questionId) => {
      return startedPromise
        .then(() => connection.invoke('LeaveQuestionGroup', questionId))
        .catch(console.error);
    };
    connection.on('AnswerAdded', (answer) => {
      questionHub.$emit('answer-added', answer);
    });

    connection.on('QuestionScoreChange', (questionId, score) => {
      questionHub.$emit('score-changed', { questionId, score });
    });
    connection.onclose(() => start());
    start();
  },
};
