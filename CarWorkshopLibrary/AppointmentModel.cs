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
            //2 while zrobić w 1
            TimeSpan timeLeft;
            TimeSpan endTime = new TimeSpan(endHour, 0, 0);
            DateTime dateTmp = date;
            int workDays = 0;
            while(estimatedTime >= (endHour-startHour))
            {
                workDays++;
                estimatedTime -= (endHour-startHour);
            }

            //dateTmp = dateTmp.AddHours(estimatedTime);
            //if (dateTmp.TimeOfDay.h > endTime )
            if (dateTmp.Hour + estimatedTime >= endHour)
            {
                timeLeft = (endTime - dateTmp.TimeOfDay);
                timeLeft = new TimeSpan((int)estimatedTime, 0, 0) - timeLeft;
                dateTmp = changeDateToNextWorkDay(dateTmp, timeLeft);
            }
            else
                dateTmp = dateTmp.AddHours(estimatedTime);

            TimeSpan time = dateTmp.TimeOfDay;
            while (workDays != 0)
            {
                dateTmp = changeDateToNextWorkDay(dateTmp);
                workDays--;
            }
            return dateTmp + time - dateTmp.TimeOfDay;
        }
        public static DateTime GetFinishDate(DateTime date, double estimatedTime)
        {
            //2 while zrobić w 1
            TimeSpan timeLeft;
            TimeSpan endTime = new TimeSpan(endHour, 0, 0);
            DateTime dateTmp = date;
            int workDays = 0;
            while (estimatedTime >= (endHour - startHour))
            {
                workDays++;
                estimatedTime -= (endHour - startHour);
            }

            //dateTmp = dateTmp.AddHours(estimatedTime);
            //if (dateTmp.TimeOfDay.h > endTime )
            if (dateTmp.Hour + estimatedTime >= endHour)
            {
                timeLeft = (endTime - dateTmp.TimeOfDay);
                timeLeft = new TimeSpan((int)estimatedTime, 0, 0) - timeLeft;
                dateTmp = changeDateToNextWorkDay(dateTmp, timeLeft);
            }
            else
                dateTmp = dateTmp.AddHours(estimatedTime);

            TimeSpan time = dateTmp.TimeOfDay;
            while (workDays != 0)
            {
                dateTmp = changeDateToNextWorkDay(dateTmp);
                workDays--;
            }
            return dateTmp + time - dateTmp.TimeOfDay;
        }
        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        public static bool isWorkDay(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                return false;
            else
                return true;
        }

        public static bool isWorkHour(DateTime dt)
        {
            if (dt.Hour >= startHour && dt.Hour < endHour)
                return true;
            else
                return false;
        }
        public static DateTime changeDateToNextWorkDay(DateTime dt) //  DateTime + TimeSpan  <- moze byc blad
        {
            TimeSpan ts = new TimeSpan(startHour, 0, 0);
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
        public static DateTime changeDateToNextWorkDay(DateTime dt, TimeSpan timeLeft) //  DateTime + TimeSpan  <- moze byc blad
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
