using System;
using System.Collections.Generic;
using System.Text;

namespace newsolartester
{
    public class Day
    {
        public string datetime { get; set; }
        public float datetimeEpoch { get; set; }
        public double tempmax { get; set; }
        public double tempmin { get; set; }
        public double temp { get; set; }
        public double feelslikemax { get; set; }
        public double feelslikemin { get; set; }
        public double feelslike { get; set; }
        public double dew { get; set; }
        public double humidity { get; set; }
        public float precip { get; set; }
        public float precipprob { get; set; }
        public object precipcover { get; set; }
        public object preciptype { get; set; }
        public float snow { get; set; }
        public float snowdepth { get; set; }
        public double windgust { get; set; }
        public double windspeed { get; set; }
        public double winddir { get; set; }
        public double pressure { get; set; }
        public double cloudcover { get; set; }
        public double visibility { get; set; }
        public double solarradiation { get; set; }
        public double solarenergy { get; set; }
        public float uvindex { get; set; }
        public float severerisk { get; set; }
        public string sunrise { get; set; }
        public float sunriseEpoch { get; set; }
        public string sunset { get; set; }
        public float sunsetEpoch { get; set; }
        public double moonphase { get; set; }
        public string conditions { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string source { get; set; }
        public List<Hour> hours { get; set; }
    }
}
