using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Models.Issues
{
    [Table("Answer")]
    public class Answer:SysField
    {
        public virtual int Id { get; set; }
        public virtual Issue Issue { get; set; }
     
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string HomePage { get; set; }
        public virtual int Ip { get; set; }

        [DisplayName("评论内容")]
        [Required]
        [MaxLength(5000 * 1024)]
        public virtual string Text { get; set; }

    }
}
