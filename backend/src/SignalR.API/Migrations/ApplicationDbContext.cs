using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SignalR.API.Models;

namespace SignalR.API.Migrations
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<Question>()
                    .HasData(
                        new Question
                        {
                            Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                            Title = "Welcome",
                            Body = "Welcome to the _mini Stack Overflow_ rip-off!\nThis will help showcasing **SignalR** and its integration with **Vue**",
                        }
                    );
                modelBuilder.Entity<Answer>()
                    .HasData(
                        new Answer
                        {
                            Id = Guid.Parse("1f4f6270-12ba-4dba-b332-e2a20512f316"),
                            QuestionId = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                            Body = "Sample answer"
                        }
                    );
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}