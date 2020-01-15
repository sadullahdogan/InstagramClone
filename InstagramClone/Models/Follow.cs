using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramClone.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual User Follower { get; set; }
        public virtual User Following { get; set; }

    }
}