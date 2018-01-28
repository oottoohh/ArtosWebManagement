using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class CardTapHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CardNumber { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }
}

