using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public enum TransactionsTypeIds
    {
        Transport=0,Merchant, Others
    }
    public class TransactionType:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name{ get; set; }
        public string Remark{ get; set; }
    }
}