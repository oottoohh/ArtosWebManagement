using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class DeviceSyncLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long DeviceId { get; set; }
        public string SyncDate { get; set; }
        public string Remark { get; set; }
        public SyncStatus Status { get; set; }
        public string IMEI { get; set; }



    }
    public enum SyncStatus { Succeed, Fail}
}


/*






 */
