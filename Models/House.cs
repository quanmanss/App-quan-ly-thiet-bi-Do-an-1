using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesControlApp.Models
{
    public class House
    {   
        public int ID { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public string Joincode { get; set; }

        public int OwnerUserID { get; set; }

        
    }
}
