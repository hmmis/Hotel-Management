using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required"), MaxLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name User is required"), MaxLength(30)]

        public string User_Name { get; set; }
        [Required(ErrorMessage = "Password is required"), MaxLength(30)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required"), MaxLength(40)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number is required"), MaxLength(15)]
        public string Mobile { get; set; }

        public string Other { get; set; }

        public int IsAdmin { get; set; }

        public List<Schedule> Schedules { get; set; }
    }
}
