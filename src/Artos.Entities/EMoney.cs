using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class EMoney
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CardName { get; set; }
        public string Provider { get; set; }

        public string LogoUrl { get; set; }
    }
}
/*





 * */
