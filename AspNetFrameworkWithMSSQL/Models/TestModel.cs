using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTechnical.Models
{
    public class TestModel
    {
        public int id { get; set; }
        public String title { get; set; }
        public String Category { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public String Location { get; set; }
        public String Description { get; set; }
        
    }
}