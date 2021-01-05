using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class WarehouseModel
    {
        public int idParts { get; set; }
        public string partName { get; set; }
        public string producent { get; set; }
        public string price { get; set; }
        public string stockQuantity { get; set; }
        //public string deliveryTime { get; set; }
    }
}
