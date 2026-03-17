using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedulink
{
    internal class Appointment
    {
        //public string professor_id { get; set; }
        public string username { get; set; }
        public string appDate { get; set; }  // Note: If you want it as a string
        public string time { get; set; }
        public string reason { get; set; }
        //DateTime appDate;
        public Appointment() { }

        public Appointment(string Username,DateTime appDate, string time, string reason)
        {
            this.username = Username;
            this.appDate = appDate.ToShortDateString();
            this.time = time;
            this.reason = reason;
        }
        

        public override string ToString()
        {
            string message = "You have a date on " + appDate + " at " + time + " for " + reason;
            return message;
        }

        
    }
}
