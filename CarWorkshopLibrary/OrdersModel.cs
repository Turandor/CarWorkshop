using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class OrdersModel
    {
        public int idOrder { get; set; }
        public int idParts { get; set; }
        public int amount { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime realizationDate { get; set; }
        public string status { get; set; }
    }
}
