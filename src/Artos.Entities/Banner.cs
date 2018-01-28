using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class Banner:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name{ get; set; }
        public string ImageUrl{ get; set; }
        public bool IsActive{ get; set; }
        public int OrderNo{ get; set; }
        public string OpenUrl{ get; set; }
       
    }
}

/*






*/
