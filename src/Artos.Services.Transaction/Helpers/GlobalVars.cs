using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artos.Services.Transaction.Helpers
{
    public class GlobalVars
    {
        public static readonly TimeSpan TicketDuration = new TimeSpan(3, 0, 0);
        public static readonly decimal MaxPrice = 5000;

    }
}
