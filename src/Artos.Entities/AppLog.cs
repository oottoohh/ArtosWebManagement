using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class AppLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id{ get; set; }
        public string Remark{ get; set; }
        public LogCategories Category { get; set; }
        public DateTime LogDate{ get; set; }
        public string UserName{ get; set; }
        public string Device{ get; set; }
        public AppTypes AppType { get; set; }
    }

    public enum LogCategories
    {
        Create, Read, Update, Delete, Others
    }
    public enum AppTypes
    {
        Mobile, BackgroundJob, WebService, Desktop, Wearable, Tablet
    }
}
