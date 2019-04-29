using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiAndSupport.Models
{
    public class SysField
    {
        #region Attribute

        /// <summary>
        /// 创建人Id
        /// </summary>
        public long CreatedId { get; set; }

        /// <summary>
        /// 创建人主键
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [MaxLength(50)]
        public string CreatedName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改人Id
        /// </summary>
        public long ModifiedId { get; set; }

        /// <summary>
        /// 修改人主键
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [MaxLength(50)]
        public string ModifiedName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        #endregion
    }
}
