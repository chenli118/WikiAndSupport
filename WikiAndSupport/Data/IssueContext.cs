using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Data
{
    public class IssueContext: DbContext
    {
        public IssueContext(DbContextOptions<IssueContext> options) : base(options) {
        }

        public virtual DbSet<Models.Issues.Issue> Issue { get; set; }
         public virtual DbSet<Models.Issues.Answer> Answer { get; set; }
        public virtual DbSet<Models.Issues.Comment> Comment { get; set; }
        public virtual DbSet<Models.Issues.Tag> Tag { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Models.Issues.Issue>().Property(p => p.ModifiedOn).IsConcurrencyToken();
            modelBuilder.Entity<Models.Issues.Answer>().Property(p => p.ModifiedOn).IsConcurrencyToken();
            modelBuilder.Entity<Models.Issues.Comment>().Property(p => p.ModifiedOn).IsConcurrencyToken();
            modelBuilder.Entity<Models.Issues.Tag>().Property(p => p.ModifiedOn).IsConcurrencyToken();
        }
    }
}
