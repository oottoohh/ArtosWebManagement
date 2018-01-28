using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class ArtosTransactionDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TicketNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public string Moda { get; set; }
        public string RouteName { get; set; }
        public long RouteId { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public decimal Amount { get; set; }
        public string QRCode { get; set; }
        public long ScannerId { get; set; }
    }
}













 
