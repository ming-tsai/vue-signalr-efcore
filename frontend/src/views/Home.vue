<template>
  <div>
    <h1>
      This totally looks like Stack Overflow
      <button
        v-b-modal.addQuestionModal
        class="btn btn-primary mt-2 float-right"
      >
        <i class="fas fa-plus" /> Ask a question
      </button>
    </h1>
    <ul class="list-group question-previews mt-4">
      <question-preview
        v-for="question in questions"
        :key="question.id"
        :question="question"
        class="list-group-item list-group-item-action mb-3"
      />
    </ul>
    <add-question-modal @question-added="onQuestionAdded" />
  </div>
</template>

<script>
import QuestionPreview from '@/components/QuestionPreview';
import AddQuestionModal from '@/components/AddQuestionModal';
import axios from 'axios';

export default {
  components: {
    QuestionPreview,
    AddQuestionModal,
  },
  data() {
    return {
      questions: [],
    };
  },
  created() {
    axios.get(`https://localhost:5001/api/question`).then((res) => {
      this.questions = res.data;
    });
  },
  methods: {
    onQuestionAdded(question) {
      this.questions = [question, ...this.questions];
    },
  },
  beforeDestroy() {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$questionHub.$off('score-changed', this.onScoreChanged);
  },
};
</script>

<style>
.question-previews .list-group-item {
  cursor: pointer;
}
</style>
