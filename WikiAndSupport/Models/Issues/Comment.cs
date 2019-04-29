using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Models.Issues
{
    [Table("Comment")]

    public class Comment:SysField
    {
        public virtual int Id { get; set; }
        public virtual Issue Issue { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string HomePage { get; set; }
        public virtual int Ip { get; set; }
        public virtual string Text { get; set; }

    }
}
