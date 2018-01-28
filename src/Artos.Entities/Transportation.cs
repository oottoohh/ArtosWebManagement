using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class Transportation:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Moda { get; set; }
        public string QRCode { get; set; }
        public string PinCode { get; set; }
        public string PoliceNo { get; set; }
        public string RouteName { get; set; }
        public long RouteId { get; set; }
        public string QRCodeInJSON { get; set; }

        public decimal Price { get; set; }
    }
}










 
