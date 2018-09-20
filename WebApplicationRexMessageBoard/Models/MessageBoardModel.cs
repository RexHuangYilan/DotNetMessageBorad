using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationRexMessageBoard.Models
{
    public class MessageBoardModel
    {
        public int ID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(255)]
        [Required]
        public string Content { get; set; }

        //[NotMapped]
        public DateTime? CreateTime { get; set; }

        //[Required]
        public string UserID { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ReplyModels> Reply { get; set; }
    }
}