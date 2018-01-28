using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class CardTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string CardNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public long TransactionTypeId { get; set; }

    }
}








