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
        public double price { get; set; }  
        public int stockQuantity { get; set; } 
    }
}
