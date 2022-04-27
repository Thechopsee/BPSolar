using System;
using System.Collections.Generic;
using System.Text;

namespace api_downloads.ForecastVissualCrossing
{
    public class CurrentConditions
    {
        public string datetime { get; set; }
        public float datetimeEpoch { get; set; }
        public double temp { get; set; }
        public double feelslike { get; set; }
        public double humidity { get; set; }
        public double dew { get; set; }
        public float precip { get; set; }
        public object precipprob { get; set; }
        public object snow { get; set; }
        public float snowdepth { get; set; }
        public object preciptype { get; set; }
        public float windgust { get; set; }
        public double windspeed { get; set; }
        public float winddir { get; set; }
        public double pressure { get; set; }
        public float visibility { get; set; }
        public double cloudcover { get; set; }
        public object solarradiation { get; set; }
        public float solarenergy { get; set; }
        public object uvindex { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string sunrise { get; set; }
        public float sunriseEpoch { get; set; }
        public string sunset { get; set; }
        public float sunsetEpoch { get; set; }
        public double moonphase { get; set; }
    }
}
