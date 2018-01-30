using System;
using System.ComponentModel.DataAnnotations;
namespace Artos.Management.Models
{
    public class EmoneyValidModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string LogoUrl { get; set; }
    }
}
