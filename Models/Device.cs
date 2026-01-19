using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesControlApp.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int RoomID { get; set; }

        public string RoomName { get; set; }

        public int Intensity { get; set; }

        // Memory-only

        public bool IsOn { get; set; } = false;
        public string ButtonLabel { get; set; }

    }

}