using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class AdminController : Controller
    {
        
        
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            User admin = (User)Session["admin"];
            User list = context.Users.SingleOrDefault(d => d.User_Name == admin.User_Name);
            return View(list);
        }

   

        public ActionResult RoomList()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Remove("admin");
            return RedirectToAction("LogIn", "Home");
        }

        //==========================================================================================Create New Admin
        [HttpGet]
        public ActionResult CreateAdmin()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(User u)
        {
            if (ModelState.IsValid)
            {
                HotelDbContext context = new HotelDbContext();
                u.IsAdmin = 1;
                context.Users.Add(u);
                context.SaveChanges();
                return RedirectToAction("index");

            }
            else
            {
                return View();
            }


        }
       

    }
}
