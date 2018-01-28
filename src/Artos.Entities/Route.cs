using System;
using System.Collections.Generic;
using System.Text;

namespace Artos.Entities
{
    public class Route:AuditAttribute
    {
        public long Id { get; set; }
        public string RouteName { get; set; }
        public string Moda { get; set; }
        public decimal Price { get; set; }
      
    }
}
/*




 */
