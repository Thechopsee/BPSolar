using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Controllers
{
    public struct Hour {
        public string hour_num;
        public double powerOutput;
    }
    public struct Days {
        public string date { get; set; }
        public List<Hour> hours { get; set; }
        public double powerOutput { get; set; }
    }
    public class ApiStructureForForecast
    {
        public ApiStructureForForecast()
        {
            days = new List<Days>();
        }
        public string? latitude;
        public string? longtitude;
        public string? mesto;
        public string? iso;
        public List<Days>? days;
    }

    public class ApiStructure3hrs
    {
        public ApiStructure3hrs()
        {
            hours = new List<Hour>();
        }
        public List<Hour> hours { get; set; }
    }
}
