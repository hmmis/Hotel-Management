using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace Hotel_Management.Controllers
{
    public class AdminCategoryController : Controller
    {

        //===============================================Catagory Crud=============================================

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            return View(context.Categories.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category cate)
        {
            if (ModelState.IsValid)
            {
                HotelDbContext context = new HotelDbContext();
                context.Categories.Add(cate);
                context.SaveChanges();
                return RedirectToAction("index");

            }
            else
            {
                return View();
            }

           
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Category category = context.Categories.SingleOrDefault(d => d.Id == id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category emp)
        {
            if (ModelState.IsValid)
            {
                HotelDbContext context = new HotelDbContext();
                Category myUpdate = context.Categories.SingleOrDefault(d => d.Id == emp.Id);

                myUpdate.CategoryName = emp.CategoryName;

                context.SaveChanges();
                return RedirectToAction("index");

            }
            else
            {
                return View();
            }
           
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Category details = context.Categories.SingleOrDefault(d => d.Id == id);
            return View(details);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            HotelDbContext context = new HotelDbContext();
            Category delEmp = context.Categories.SingleOrDefault(d => d.Id == id);
            return View(delEmp);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_Post(int id)
        {
            HotelDbContext context = new HotelDbContext();
            Category myDelete = context.Categories.SingleOrDefault(d => d.Id == id);
            context.Categories.Remove(myDelete);
            context.SaveChanges();

            return RedirectToAction("Index");
        } 
    }
}
