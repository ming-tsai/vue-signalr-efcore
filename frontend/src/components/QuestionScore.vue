<template>
  <h3 class="text-center scoring">
    <button
      class="btn btn-link btn-lg p-0 d-block mx-auto"
      @click.stop="onUpvote"
    >
      <i class="fas fa-sort-up" />
    </button>
    <span class="d-block mx-auto">{{ question.score }}</span>
    <button
      class="btn btn-link btn-lg p-0 d-block mx-auto"
      @click.stop="onDownvote"
    >
      <i class="fas fa-sort-down" />
    </button>
  </h3>
</template>
<script>
import axios from 'axios';
export default {
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  created() {
    this.$questionHub.$on('score-changed', this.onScoreChanged);
  },
  methods: {
    onUpvote() {
      axios
        .patch(`https://localhost:5001/api/question/${this.question.id}/upvote`)
        .then((res) => {
          Object.assign(this.question, res.data);
        });
    },
    onDownvote() {
      axios
        .patch(
          `https://localhost:5001/api/question/${this.question.id}/downvote`
        )
        .then((res) => {
          Object.assign(this.question, res.data);
        });
    },
    onScoreChanged({ questionId, score }) {
      if (this.question.id !== questionId) return;
      Object.assign(this.question, { score });
    },
  },
};
</script>

<style scoped>
.scoring .btn-link {
  line-height: 1;
}
</style>
