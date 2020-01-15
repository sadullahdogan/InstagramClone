using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramClone.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
    }
}