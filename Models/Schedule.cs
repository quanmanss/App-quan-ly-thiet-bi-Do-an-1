using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesControlApp.Models
{
    public class Schedule
    {
        public int ID { get; set; }
        public int DeviceID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
