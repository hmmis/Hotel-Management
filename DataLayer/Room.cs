using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Room No is required")]
        public string RoomNo { get; set; }

        [MaxLength(100)]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Room Details is required"), MaxLength(1000)]
        public string Details { get; set; }

        [Required(ErrorMessage = "Every Room Has a Category is required")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public List<Schedule> Schedules { get; set; }
    }
}
