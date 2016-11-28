using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date), Required(ErrorMessage = "Check In Date Requred")]
        public DateTime Checked_In { get; set; }

        [DataType(DataType.Date), Required(ErrorMessage = "Check In Date Requred")]
        public DateTime Checked_Out { get; set; }

        [Required(ErrorMessage = "Room No is Requred")]
        public int Room_Id { get; set; }

        [Required(ErrorMessage = "Valied User is Requred")]
        public int User_Id { get; set; }

        public int IsHistoryRole { get; set; }

        [ForeignKey("User_Id")]
        public User User { get; set; }

        [ForeignKey("Room_Id")]
        public Room Room { get; set; }

    }
}
