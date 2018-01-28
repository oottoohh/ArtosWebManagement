using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class UserDevice:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string IMEI { get; set; }
        public MobileDeviceType DeviceType { get; set; }
        public bool IsActive { get; set; }
        
    }
    public enum MobileDeviceType
    {
        SmartPhone=0, FeaturedPhone, Tablet
    }
}
/*






 */
