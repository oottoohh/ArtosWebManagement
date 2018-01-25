using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Artos.Management.Models
{
    public class TransportRouteValidModel
    {
        [Required]
        public string NameRoute { get; set; }

        [Required]
        public string Moda { get; set; }

        [Required]
        public string Price { get; set; }
    }
}
