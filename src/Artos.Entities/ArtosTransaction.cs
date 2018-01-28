using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class ArtosTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TicketNo { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalWithoutDisc { get; set; }
        public int TransactionTypeId { get; set; }
    }
}

/*










 */
