using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class UserProfile:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CardID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PIN { get; set; }
        public string PicUrl { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
/*
 * 










 */
