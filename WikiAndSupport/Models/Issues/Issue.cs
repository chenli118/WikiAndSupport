using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Models.Issues
{
    [Table("Issue")]
    public class Issue:SysField
    {
        public Issue()
        {
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
        }
        [Key] 
        public int IssueId { get; set; }

        [DisplayName("问题标题")]
        [Required]
        public string Title { get; set; }
        [DisplayName("问题描述")]
        [Required]
        [MaxLength(5000*1024)]
        public string Content { get; set; }
        [DisplayName("提问人")]
        public string CommitBy { get; set; }
        [DisplayName("提出时间")]
        public DateTime CommitOn { get; set; }
        public virtual ICollection<Answer> Answers { get; private set; }

        public virtual ICollection<Comment> Comments { get; private set; }
    }
}
