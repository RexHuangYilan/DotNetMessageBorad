using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationRexMessageBoard.Models
{
    public class ReplyModels
    {
        public int ID { get; set; }

        [Key]
        [Required]
        public virtual MessageBoardModel MessageBoard { get; set; }

        [Key]
        [Required]
        public virtual MessageModels Message { get; set; }
    }
}