using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InstagramClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        public DateTime Date { get; set; }
        public string PostText { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }


        public virtual Image Image { get; set; }
        public virtual User User { get; set; }
        
    }
}