using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoRESTService.Models
{
    public class SystemSetting
    {
        public int ID { get; set; }
        public string SERVICENAME { get; set; }
        public string STARTTIME { get; set; }
        public string ENDTIME { get; set; }
        public DateTime SD { get; set; }
        public DateTime ED { get; set; }
        public bool WEEKLY { get; set; }
        public string FEQUENCY { get; set; }
    }
}