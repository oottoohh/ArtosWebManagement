using System;
namespace Artos.Entities
{
    public class TicketGenerator
    {
        //Moda|userid|date(yyyyMMdd)|time(hhmmss)
        public TicketGenerator(string Moda, long UserId, DateTime TicketDate)
        {
            this.Moda = Moda;
            this.UserId = UserId;
            this.TicketDate = TicketDate;
        }

        public string Moda
        {
            get;
            set;
        }
        public long UserId
        {
            get;
            set;
        }
        public DateTime TicketDate
        {
            get;
            set;
        }
        public string GetTicketNo(){
            return $"{Moda}-{UserId}-{TicketDate.ToString("yyyyMMdd")}-{TicketDate.ToString("HHmmss")}";

        }
    }
}
