using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramClone.Controllers
{
    public class HomeController : Controller
    {
        DAO.DatabaseContext db =  new DAO.DatabaseContext();

        public ActionResult EditPost(int postId ){
            var post = db.Posts.Single(x => x.Id == postId);

            return View(post);
        }
        public ActionResult ShowProfile(int? id) {
            
            int userId = Convert.ToInt32(Session["User"]);
            if (id == null)
                id = userId;
            var user = db.Users.Single(x => x.ID == id);
            ViewBag.User = user;
            var posts = db.Posts.Where(x => x.User.ID ==id ).ToList();
            for(int i = 0; i < posts.Count; i++)
            {
                posts[i].LikeCount = getCount(posts[i].Id);
            }
            List<int> LikedPosts = db.Likes.Where(x => x.User.ID == userId).Select(x => x.Post.Id).ToList();
         
            ViewBag.LikedPosts = LikedPosts;
            ViewBag.Posts = posts;
            return View();
        }
        [HttpPost]
        public ActionResult EditPost(Post p) {
            var post = db.Posts.Single(x => x.Id == p.Id);
            post.PostText = p.PostText;
            if (p.File != null)
            {
                Image image = new Image();
                int userId = Convert.ToInt32(Session["User"]);
                var user = db.Users.Single(x => x.ID == userId);
                string DosyaAdi = post.File.FileName;
                var yuklemeYeri = Path.Combine(Server.MapPath("~/Images"), DosyaAdi);
                string imagePath = "~/Images/" + DosyaAdi;
                image.ImagePath = imagePath;
                image.ImageName = DosyaAdi;
                post.File.SaveAs(yuklemeYeri);
                db.Images.Add(image);
                db.SaveChanges();
                post.Image = image;
            }
            db.SaveChanges();

            
            return RedirectToAction("Index", "Home");
        }
      
        public ActionResult Like(int postId) {
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            var post = db.Posts.Single(x => x.Id == postId);
            Likes likes = new Likes();
            likes.Date = DateTime.Now;
            likes.Post = post;
            likes.User = user;
            db.Likes.Add(likes);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Dislike(int postId) {
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            var post = db.Posts.Single(x => x.Id == postId);
            
            var like = db.Likes.Single(x => x.Post.Id == post.Id && x.User.ID == user.ID);
            db.Likes.Remove(like);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Index()
        {
            var post = db.Posts.Single(x => x.Id == 1);
            
            if (Session["User"] == null)
                return RedirectToAction("../User/Login");
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            List<int> LikedPosts = db.Likes.Where(x => x.User.ID == userId).Select(x=>x.Post.Id).ToList();
            List<int> followingList = db.Follows.Where(x => x.Follower.ID == userId).Select(x => x.Following.ID).ToList();
            ViewBag.LikedPosts = LikedPosts;
     
                       
            var posts = db.Posts.Where(x => x.User.ID == userId || followingList.Contains(x.User.ID)).ToList();
            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].LikeCount = getCount(posts[i].Id);
            }
            ViewBag.Posts = posts;
            return View();
        }
        private int getCount(int id)
        {
            return db.Likes.Where(a => a.Post.Id == id).Count();
        }
        public ActionResult About()
        {
            if (Session["User"] == null)
                return RedirectToAction("../User/Login");

            int userId = Convert.ToInt32(Session["User"]);
            
            return RedirectToAction("ShowProfile", "Home", new { id=userId });
        }
        public ActionResult CreatePost() {
            return View();
        }
        public ActionResult Profile() {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePost(Post post) {
            if (Session["User"] == null)
                return RedirectToAction("../User/Login");
            Image image = new Image();
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            string DosyaAdi = post.File.FileName;
            var yuklemeYeri = Path.Combine(Server.MapPath("~/Images"), DosyaAdi);
            string imagePath = "~/Images/" + DosyaAdi;
            image.ImagePath = imagePath;
            image.ImageName = DosyaAdi;
            post.File.SaveAs(yuklemeYeri);
            db.Images.Add(image);
            db.SaveChanges();
            post.Image = image;
            post.Date = DateTime.Now;
            post.User = user;
            db.Posts.Add(post);
            db.SaveChanges();
            
           
            
            return View();
        }
        
        public ActionResult Follow(int id) {
            Follow folow = new Follow();
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            folow.Date = DateTime.Now;
            folow.Follower = user;
            folow.Following = db.Users.Single(x => x.ID == id);

            db.Follows.Add(folow);
            db.SaveChanges();
            return RedirectToAction("ListOtherUsers", "Home");
        }
        
        public ActionResult Unfollow(int id) {
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            var following= db.Users.Single(x => x.ID == id);
            var follow = db.Follows.Single(x => x.Follower.ID == user.ID && x.Following.ID == following.ID);
            db.Follows.Remove(follow);
            db.SaveChanges();


            return RedirectToAction("ListOtherUsers", "Home");

        }
        public ActionResult ListOtherUsers() {
            if (Session["User"] == null)
                return RedirectToAction("../User/Login");
       
            int userId = Convert.ToInt32(Session["User"]);
            var user = db.Users.Single(x => x.ID == userId);
            List<int> followingList = db.Follows.Where(x => x.Follower.ID == userId).Select(x => x.Following.ID).ToList();
            ViewBag.followingList = followingList;
            var listUsers = db.Users.Where(x => x.ID != userId).ToList();
            return View(listUsers);
        }

        public ActionResult Contact()
        {
            if (Session["User"] == null)
                return RedirectToAction("../User/Login");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}