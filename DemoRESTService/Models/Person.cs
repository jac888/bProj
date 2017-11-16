using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLib;

namespace DemoRESTService.Models
{
    public class Person
    {
        //public long ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}