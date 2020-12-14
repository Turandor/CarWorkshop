using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class EmployeeModel
    {
        public int idEmployee { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string specialization { get; set; }

        public string fullInfromation
        {
            get
            {
                return $"{ firstName } { lastName } { specialization }";
            }
        }
    }
}
