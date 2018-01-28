using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artos.Entities
{
    public class StaticContent:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public StaticContentType ContentType { get; set; }

    }

    public enum StaticContentType
    {
        PageUrlContent=0, StringContent, ImageContent, HtmlContent
    }
}
/*




 */
