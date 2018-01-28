using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class TicketPool
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TicketNo{ get; set; }
        public string Moda{ get; set; }
        public long TransportId{ get; set; }
        public bool IsActive{ get; set; }
        public long UserId{ get; set; }
        public string UserName{ get; set; }
        public DateTime GeneratedDate { get; set; }
        
    }
}










