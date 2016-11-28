using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DbSeeder : DropCreateDatabaseIfModelChanges<HotelDbContext>
    {
        protected override void Seed(HotelDbContext context)
        {
            base.Seed(context);
            User d = new User()
            {
                Name ="Admin",
                User_Name = "admin",
                Password = "admin",
                IsAdmin = 1,
                Mobile = "1545445",
                Email = "admin@admin.com"
            };
            context.Users.Add(d);
            context.SaveChanges();
        }
    }
}
