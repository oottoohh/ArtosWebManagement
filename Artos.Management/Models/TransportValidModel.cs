using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Artos.Management.Models
{
    public class TransportValidModel
    {
        [Required]
        public string TransportName { get; set; }

        [Required]
        public string Moda { get; set; }

        [Required]
        public string QRCode { get; set; }
        
        [Required]
        public string PoliceNo { get; set; }

        [Required]
        public string RouteName { get; set; }

        [Required]
        public string RouteID { get; set; }
        [Required]
        public string QRCodeinJson { get; set; }
    
        [Required]
        public string PinCode { get; set; }
        [Required]
        public string Price { get; set; }
        
    }
}
