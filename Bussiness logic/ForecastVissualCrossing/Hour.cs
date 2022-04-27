using System;
using System.Collections.Generic;
using System.Text;

namespace api_downloads.ForecastVissualCrossing
{
    public class Hour
    {
        public string datetime { get; set; }
        public float datetimeEpoch { get; set; }
        public double temp { get; set; }
        public double feelslike { get; set; }
        public double humidity { get; set; }
        public double dew { get; set; }
        public float precip { get; set; }
        public float? precipprob { get; set; }
        public float? snow { get; set; }
        public float? snowdepth { get; set; }
        public object preciptype { get; set; }
        public double? windgust { get; set; }
        public double windspeed { get; set; }
        public double winddir { get; set; }
        public double pressure { get; set; }
        public double visibility { get; set; }
        public double cloudcover { get; set; }
        public float solarradiation { get; set; }
        public double solarenergy { get; set; }
        public float uvindex { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string source { get; set; }
        public float? severerisk { get; set; }
    }
}
