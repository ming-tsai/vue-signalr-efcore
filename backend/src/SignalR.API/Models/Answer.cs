using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR.API.Models
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
    }
}