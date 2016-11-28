using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
          
           
            User user= (User)Session["user"];
            User list = context.Users.SingleOrDefault(d => d.User_Name == user.User_Name);
            return View(list);
        }

        
        [HttpGet]
        public ActionResult BookNewRoom()
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            return RedirectToAction("Index", "Home");
            
        }

        public ActionResult BookedList() {

            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();

            User user = (User)Session["user"];
            List<Schedule> list = context.Schedules.Include("User").Include("Room.Category").Where(u => u.User.User_Name.Equals(user.User_Name) && u.IsHistoryRole == 0).OrderBy( u => u.Checked_In).ToList();

            return View(list);
        }
        public ActionResult OldBookedListHistory()
        {

            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();

            User user = (User)Session["user"];
            List<Schedule> list = context.Schedules.Include("User").Include("Room.Category").Where(u => u.User.User_Name.Equals(user.User_Name) && u.IsHistoryRole == 1).OrderBy(u => u.Checked_In).ToList();

            return View(list);
        }


        public ActionResult LogOff()
        {
            Session.Remove("user");
            return RedirectToAction("LogIn", "Home");
        }
        

    }
}
