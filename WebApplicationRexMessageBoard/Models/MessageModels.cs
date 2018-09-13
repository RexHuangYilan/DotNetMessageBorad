using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationRexMessageBoard.Models
{
    public class MessageModels
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ReplyModels> Reply { get; set; }
    }
}