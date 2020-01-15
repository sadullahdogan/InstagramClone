using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InstagramClone.Models
{
    public class User
    {
        public int ID { get; set; }
        [DisplayName("Adınız :"),Required]
        public string Name { get; set; }
        [DisplayName("Soy Adınız :"), Required]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı :"), Required]
        public string  Username { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        [DisplayName("E Mail Adresi :"), Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Şifre :"), Required,DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
        [DisplayName("Şifre (Tekrar) :"), Required,DataType(DataType.Password),Compare("Password"),NotMapped]
        public string Repassword { get; set; }
        public virtual Image Image { get; set; }



    }
}