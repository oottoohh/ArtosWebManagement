using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class ContactCenter:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id{ get; set; }
        public string Name{ get; set; }
        public string Phone{ get; set; }
        public string Email{ get; set; }
        public string Remark{ get; set; }
    }
}