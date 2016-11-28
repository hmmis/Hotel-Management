using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class AdminScheduleController : Controller
    {
        //
     
        [NonAction]
        public List<SelectListItem> GetRoomList()
        {
            HotelDbContext context = new HotelDbContext();
            List<SelectListItem> dlist = new List<SelectListItem>();
            foreach (Room d in context.Roomes.ToList())
            {
                SelectListItem li = new SelectListItem();
                li.Text = d.RoomNo;
                li.Value = d.Id.ToString();
                dlist.Add(li);
            }
            return dlist;

        }
        [HttpGet]
        public ActionResult Index()
        {
            //======================================================Show [Booked List]
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            List<Schedule> list = context.Schedules.Include("User").Include("Room.Category").Where(u => (u.IsHistoryRole == 0)).ToList();

            return View(list);
        }
        [HttpGet]
        public ActionResult BookHistory()
        {
            //======================================================Show Book History
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            List<Schedule> list = context.Schedules.Include("User").Include("Room.Category").Where(u => (u.IsHistoryRole == 1)).ToList();

            return View(list);
        }
        //==========================================================================================Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Schedule s = context.Schedules.SingleOrDefault(d => d.Id == id);
            ViewBag.dlist = this.GetRoomList();
            return View(s);
        }
        [HttpPost]
        public ActionResult Edit(Schedule Sec)
        {

            HotelDbContext context = new HotelDbContext();
            Schedule myUpdate = context.Schedules.SingleOrDefault(d => d.Id == Sec.Id);

            //myUpdate.Room_Id = cate.Room_Id;
            myUpdate.Room_Id = Convert.ToInt32(Request["dept"]);

            context.SaveChanges();
            return RedirectToAction("index");
        }
        //==========================================================================================Check Out
        [HttpGet]
        public ActionResult CheckOut(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Schedule list = context.Schedules.SingleOrDefault(d => d.Id == id);
            return View(list);
        }
        [HttpPost]
        [ActionName("CheckOut")]
        public ActionResult ConfirmCheckOut(int id)
        {

             HotelDbContext context = new HotelDbContext();
             Schedule myUpdate = context.Schedules.SingleOrDefault(d => d.Id == id);

             myUpdate.IsHistoryRole = 1;

             context.SaveChanges();
             return RedirectToAction("index");
        }

        //==========================================================================================User Details
        [HttpGet]
        public ActionResult UserDetails(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            User list = context.Users.SingleOrDefault(d => d.Id == id);
            return View(list);
        }

        //==========================================================================================Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Schedule delSch = context.Schedules.SingleOrDefault(d => d.Id == id);
            return View(delSch);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_Post(int id)
        {
            HotelDbContext context = new HotelDbContext();
            Schedule myDelete = context.Schedules.SingleOrDefault(d => d.Id == id);
            context.Schedules.Remove(myDelete);
            context.SaveChanges();

            return RedirectToAction("Index");
        } 
    }
}
