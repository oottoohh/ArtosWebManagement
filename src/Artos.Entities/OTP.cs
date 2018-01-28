using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class OTP
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Number { get; set; }
        public string Phone { get; set; }
    
        public OTPTypes OTPType { get; set; }
        public bool IsValidated { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int Attempt { get; set; }
        public DateTime CreatedDate { get; set; }
   

    }

    public enum OTPTypes
    {
        Login=0, Register, Others
    }
}
/*









 */
