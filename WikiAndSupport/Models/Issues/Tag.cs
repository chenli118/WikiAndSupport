using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Models.Issues
{
    [Table("Tag")]
    public class Tag:SysField
    {
        public virtual int Id { get; set; }
        public virtual string Catalogue { get; set; }
        public virtual string Name { get; set; }
    }
}
