using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalR.API.Models
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}