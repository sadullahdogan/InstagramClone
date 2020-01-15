using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InstagramClone.DAO
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() {
            Database.Connection.ConnectionString = "Server=.; Database= InstagramCloneDB; Integrated Security=True;";
        }
       
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Likes> Likes { get; set; }
        
    }
}