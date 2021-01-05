using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshopLibrary
{
    public class AppointmentModel
    {
        public int idAppointment { get; set; }
        public int idCar { get; set; }
        public int idWorkplace { get; set; }
        public int idEmployee { get; set; }
        public DateTime date { get; set; }
        public string appointmentType { get; set; }
        public double cost { get; set; }
        public double estimatedTime { get; set; }
        public string neededParts { get; set; }
    }
}
