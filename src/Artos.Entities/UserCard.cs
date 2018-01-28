using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class UserCard : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CardNumber { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public decimal Balance { get; set; }
        public string CardName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }


    }
}

/*










*/
