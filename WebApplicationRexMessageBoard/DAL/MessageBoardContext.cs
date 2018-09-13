using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApplicationRexMessageBoard.Models;

namespace WebApplicationRexMessageBoard.DAL
{
    public class MessageBoardContext : DbContext
    {
        public MessageBoardContext() : base("RexMessageBoard")
        {
        }

        public DbSet<MessageBoardModel> MessageBoards { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ReplyModels>().HasKey(d => new { d.MessageBoard, d.Message });
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}