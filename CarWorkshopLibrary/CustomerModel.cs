using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class CustomerModel
    {

        public int idCustomer { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string adress { get; set; }

        public string fullInfromation
        {
            get
            {
                return $"{ firstName } { lastName } { phoneNumber } { adress }";
            }
        }
    }
}
