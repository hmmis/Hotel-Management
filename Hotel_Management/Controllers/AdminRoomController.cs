using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class AdminRoomController : Controller
    {

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
        //===============================================Room Crud=============================================

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            List<Room> deptlist = context.Roomes.Include("Category").ToList();
            return View(deptlist);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }            
            
            ViewBag.clist = this.GetCategoryList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Room room, HttpPostedFileBase file)
        {
            HotelDbContext context = new HotelDbContext();
            //----------------------------------------------------image upload -------------------
            

            //--------------------------------------------------------------------------------------

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/img/") + file.FileName);
                    room.Picture = file.FileName;
                }
                room.CategoryId = Convert.ToInt32(Request["CategoryId"]);
                context.Roomes.Add(room);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create");
            
            
        }
       [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Room r = context.Roomes.SingleOrDefault(d => d.Id == id);
            ViewBag.clist = this.GetCategoryList();
            return View(r);
        }

        [HttpPost]
       public ActionResult Edit(Room room, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/img/") + file.FileName);
                    room.Picture = file.FileName;
                }
                HotelDbContext context = new HotelDbContext();
                Room myUpdate = context.Roomes.SingleOrDefault(d => d.Id == room.Id);

                myUpdate.Details = room.Details;
                myUpdate.CategoryId = Convert.ToInt32(Request["CategoryId"]);

                context.SaveChanges();
            }
            return RedirectToAction("Index");           

            
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Room deptlist = context.Roomes.Include("Category").SingleOrDefault(d => d.Id == id);
            return View(deptlist);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Room delEmp = context.Roomes.Include("Category").SingleOrDefault(d => d.Id == id);
            return View(delEmp);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_Post(int id)
        {
            HotelDbContext context = new HotelDbContext();
            Room myDelete = context.Roomes.SingleOrDefault(d => d.Id == id);
            context.Roomes.Remove(myDelete);
            context.SaveChanges();

            return RedirectToAction("Index");
        } 
    }
}
