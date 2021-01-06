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

        public static int startHour = 8;
        public static int endHour = 18;

        public DateTime GetFinishDate()
        {
            TimeSpan timeLeft;
            DateTime dateTmp = date.AddHours(estimatedTime);
            TimeSpan endTime = new TimeSpan(endHour, 0, 0);
            if (dateTmp.TimeOfDay > endTime)
            {
                timeLeft = dateTmp.TimeOfDay - endTime;
                return changeDateToNextWorkDay(dateTmp, timeLeft);
            }
            else return dateTmp;
        }
        public static DateTime GetFinishDate(DateTime date, double estimatedTime)
        {
            TimeSpan timeLeft;
            DateTime dateTmp = date.AddHours(estimatedTime);
            TimeSpan endTime = new TimeSpan(endHour, 0, 0);
            if (dateTmp.TimeOfDay > endTime)
            {
                timeLeft = dateTmp.TimeOfDay - endTime;
                return changeDateToNextWorkDay(dateTmp, timeLeft);
            }
            else return dateTmp;
        }
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        static bool isWorkDay(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                return false;
            else
                return true;
        }

        static bool isWorkHour(DateTime dt)
        {
            if (dt.Hour >= startHour && dt.Hour < endHour)
                return true;
            else
                return false;
        }
        static DateTime changeDateToNextWorkDay(DateTime dt) //  DateTime + TimeSpan  <- moze byc blad
        {
            TimeSpan ts = new TimeSpan(startHour, 0, 0);
            if (isWorkDay(dt))
            {
                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    return dt.AddDays(3) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
            else
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    return dt.AddDays(2) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
        }
        static DateTime changeDateToNextWorkDay(DateTime dt, TimeSpan timeLeft) //  DateTime + TimeSpan  <- moze byc blad
        {
            TimeSpan ts = new TimeSpan(startHour, 0, 0) + timeLeft;
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            
            if (isWorkDay(dt))
            {
                if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    return dt.AddDays(3) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
            else
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    return dt.AddDays(2) + ts;
                }
                else
                {
                    return dt.AddDays(1) + ts;
                }
            }
        }
    }
}
