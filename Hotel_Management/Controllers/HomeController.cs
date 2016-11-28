using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
       
        public ActionResult Index()
        {
            ViewBag.clist = GetCategoryList();
            return View();
        }
        
        [HttpGet]
        public ActionResult FindHotelRoom([Bind(Exclude = "User_Id,IsHistoryRole")]Schedule schedule)
        {
            //========================Get Result From home index & show result in findHotelRoomView===================
            HotelDbContext context = new HotelDbContext();

            if (schedule.Checked_In < schedule.Checked_Out)
            {
                string category_id = Request["CategoryId"];

                Session["Checked_In"] = schedule.Checked_In;
                Session["Checked_Out"] = schedule.Checked_Out;

                ViewBag.startDate = schedule.Checked_In;
                ViewBag.endDate = schedule.Checked_Out;
                List<Room> Room = context.Roomes.Include("Category").Where(r => r.CategoryId.ToString().Equals(category_id)).ToList();
                List<Schedule> RoomList = context.Schedules.Include("Room.Category").Where(d => (schedule.Checked_In >= d.Checked_In && schedule.Checked_In < d.Checked_Out)
                    || (schedule.Checked_Out > d.Checked_In && schedule.Checked_Out <= d.Checked_Out) && d.Room.CategoryId.ToString().Equals(category_id)).ToList();

                List<int> tempIdList = RoomList.Select(q => q.Room_Id).ToList();
                
                List<Room> dropRoom = Room.Where(q => tempIdList.Contains(q.Id)).ToList();

                List<Room> list = Room.Except(dropRoom).ToList();

                if(list.Count==0){
                    ViewBag.Message = "No Result Found";
                    return View(list);
                }
                return View(list);
            }
            else {
                ViewBag.clist = GetCategoryList();
                ViewBag.Message = "Checked Out Will be Larger then Checked in";
                return View("index");
            } 
        }

        [HttpGet]
        public ActionResult RoomBook(int id) 
        {
            ViewBag.startDate = Session["Checked_In"];
            ViewBag.endDate = Session["Checked_Out"];

            Session["Room_Id"] = id;
            if (Session["admin"] != null) 
            {
                return RedirectToAction("AdminRoomBook", new { id = id });
            }

            if (Session["user"] == null)
            {
                ViewBag.id = id;
                return View();
            }
            else {
                return RedirectToAction("RoomBookConfirm");
            }
        }

        [HttpPost]
        public ActionResult RoomBook(User user)
        {
            HotelDbContext context = new HotelDbContext();

            User LoggedUser = context.Users.SingleOrDefault(u => u.User_Name == user.User_Name && u.Password == user.Password && u.IsAdmin==0);


            if (LoggedUser != null)
            {
                Session["user"] = LoggedUser;
                return RedirectToAction("RoomBookConfirm");
            }
            //----------------------------------------login Failed
            ViewBag.Message = "Invalid Username or Password";
            return RedirectToAction("RoomBook");
           
        }
        [HttpGet]
        public ActionResult AdminRoomBook(int id) 
        {
            if (Session["admin"] == null)
            {
                return View("Index");
            }
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult AdminRoomBook(User user)
        {
            HotelDbContext context = new HotelDbContext();

            User LoggedUser = context.Users.SingleOrDefault(u => u.User_Name == user.User_Name && u.IsAdmin == 0);


            if (LoggedUser != null)
            {
                Session["user"] = LoggedUser;
                return RedirectToAction("RoomBookConfirm");
            }
            //----------------------------------------login Failed
            ViewBag.Message = "Invalid Username or Password";
            return RedirectToAction("AdminRoomBook");

        }


        [HttpGet]
        public ActionResult RoomBookConfirm() {

             if (Session["user"] != null)
            {

                int roomId = Convert.ToInt32(Session["Room_Id"]);
    
                HotelDbContext context = new HotelDbContext();
                Room room = context.Roomes.Include("Category").SingleOrDefault(d => d.Id == roomId); 
            
             
                ViewBag.CheckedIn = Session["Checked_In"];
                ViewBag.CheckedOut = Session["Checked_Out"];
                User user = (User)Session["user"];

                ViewBag.userName = user.User_Name;

               
                return View(room);
            }
             return RedirectToAction("Index");
        }

        public ActionResult RoomBookFinal()
        {
            if (Session["user"] != null)
            {
                HotelDbContext context = new HotelDbContext();
              
                DateTime checked_In= (DateTime)Session["Checked_In"];
                DateTime checked_Out = (DateTime)Session["Checked_Out"];
                int RoomId = Convert.ToInt32(Session["Room_Id"]);
    
                User user = (User)Session["user"];

                Schedule schedule = new Schedule()
                {
                    Checked_In = checked_In,
                    Checked_Out = checked_Out,
                    Room_Id = RoomId,
                    User_Id = user.Id,
                    IsHistoryRole = 0
                };

                context.Schedules.Add(schedule);
                context.SaveChanges();
                ViewBag.userName = user.Name;

                if (Session["admin"] != null)
                {
                    Session.Remove("user");
                    return RedirectToAction("Index");
                }
                return View();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult LogIn()
        {
            
             return View("LogInView");

        }

        [HttpPost]
        public ActionResult LogInAction(User login)
        {            
            HotelDbContext context = new HotelDbContext();

            User LoggedUser = context.Users.SingleOrDefault(u=>u.User_Name == login.User_Name && u.Password ==login.Password);


            if (LoggedUser != null)
            {
                //----------------------------------------success user login
                if (LoggedUser.IsAdmin == 0)
                {
                    Session["user"] = LoggedUser;
                    
                    return RedirectToAction("Index", "User");
                }
       
                //----------------------------------------success admin login
                else if (LoggedUser.IsAdmin == 1)
                {
                    Session["admin"] = LoggedUser;
                    return RedirectToAction("Index", "Admin");
                }
            }
                    //----------------------------------------login Failed
            ViewBag.Message = "Invalid Username or Password";
            return View("LogInView");
            
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            
            return View("SignUp");
        }

        [HttpPost]
        public ActionResult SignUp(User u)
        {

            HotelDbContext context = new HotelDbContext();
            if (ModelState.IsValid)
            {
                context.Users.Add(u);
                context.SaveChanges();
                return View("SignUpSuccess");

            }
            else
            {
                return View();
            }
       
        }

        [NonAction]
        public List<SelectListItem> GetCategoryList()
        {
            HotelDbContext context = new HotelDbContext();
            List<SelectListItem> clist = new List<SelectListItem>();
            foreach (Category c in context.Categories.ToList())
            {
                SelectListItem li = new SelectListItem();
                li.Text = c.CategoryName;
                li.Value = c.Id.ToString();
                clist.Add(li);
            }
            return clist;
        }
        
    }
}
