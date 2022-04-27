using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Bussiness_logic.forecast.valApi
{
    public class Data
    {
        public DateTime date { get; set; }
        public double pv_voltage { get; set; }
        public double batt_charging { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public List<Data> data { get; set; }
    }

    
}
