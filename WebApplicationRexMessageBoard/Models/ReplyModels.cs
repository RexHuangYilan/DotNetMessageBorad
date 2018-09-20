using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationRexMessageBoard.Models
{
    public class ReplyModels
    {
        [Key, Column(Order = 0)]
        [Required]
        public int MessageBoardID { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public int MessageID { get; set; }
        
        public virtual MessageBoardModel MessageBoard { get; set; }

        public virtual MessageModels Message { get; set; }
    }
}