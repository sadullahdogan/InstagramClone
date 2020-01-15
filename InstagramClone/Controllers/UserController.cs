using InstagramClone.DAO;
using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramClone.Controllers
{
    public class UserController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        // GET: User
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            ModelState.Remove("Email");
            ModelState.Remove("Name");
            ModelState.Remove("Surname");
            ModelState.Remove("Repassword");
            if (ModelState.IsValid) {
                var result=db.Users.Where(x => x.Username.Equals(user.Username) &&
                x.Password.Equals(user.Password)).FirstOrDefault();
                if (result != null)
                {

                    Session["User"] = result.ID;
                    return RedirectToAction("Index", "Home");

                }
                else {

                    return View(user);
                }
            }

            return View(user);
        }


        public ActionResult Register() {

            return View();
        }
        [HttpPost]
        public ActionResult Register(User user) {
            if (ModelState.IsValid) {
                if (user.File != null) { 
                Image image = new Image();
                
               
                string DosyaAdi = user.File.FileName;
                var yuklemeYeri = Path.Combine(Server.MapPath("~/Images"), DosyaAdi);
                string imagePath = "~/Images/" + DosyaAdi;
                image.ImagePath = imagePath;
                image.ImageName = DosyaAdi;
                user.File.SaveAs(yuklemeYeri);
                db.Images.Add(image);
                db.SaveChanges();
                user.Image = image;
                }
                db.Users.Add(user);
                db.SaveChanges();
                Session["User"]=user.ID;
                return RedirectToAction("Index","Home");
            }

            return View(user);
        }
        public ActionResult Logout() {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");


        }

    }
}