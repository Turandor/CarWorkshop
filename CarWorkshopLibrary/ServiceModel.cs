using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class ServiceModel
    {
        public int idService { get; set; }
        public string serviceName { get; set; }
        public double price { get; set; }
        public string serviceCategory  { get; set; }
    }
}
