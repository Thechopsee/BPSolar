using CoordinateSharp;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace newsolartester
{
    public struct predictFormatTest
    {
        public double cislo;
        public string datum;
        public double azimuth;
        public double altitude;
        public double solarenrgy;
        public double mraky;
        public double osvit;
        public override string ToString()
        {
            return datum +" "+azimuth+" "+altitude+" "+solarenrgy+" "+mraky+ " "+osvit+":" + cislo;
        }
    }
    public struct predictionFormat
    {
        public double cislo;
        public string datum;
        public override string ToString()
        {
            return datum + ":" + cislo;
        }
    };
    class Start {
        Root forecast7Days;
        string Longitude = "18.611";
        string Latitude = "49.754667";

        int native_tilt = 30;
        int native_azimuth = -5;
        double native_wattpeek = 95;

        public Start()
        {
            updateFullData();
            List<predictFormatTest> list = GetAtributes();
            Console.WriteLine("hodina azimutsun watty");
            Console.WriteLine(string.Join("\n", list));

        }
        static public IRestResponse getDatafromVCWeather(string lat, string lon)
        {
            string start = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
            start += lon + "%2C" + lat;
            start += "?unitGroup=metric&key=NKHJXB5LAXMJGA7N6DV8BCVDL&options=nonulls";
            var client = new RestClient(start);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        public bool updateFullData(bool city = false)
        {
            IRestResponse response = getDatafromVCWeather(this.Longitude, this.Latitude);

            this.forecast7Days = JsonConvert.DeserializeObject<Root>(response.Content.ToString());


            return true;
        }
        private List<predictFormatTest> GetAtributes()
        {
            DateTime date = DateTime.Parse(forecast7Days.days[0].datetime);
            List<predictFormatTest> list = new List<predictFormatTest>();
            for (int i = 0; i < forecast7Days.days[0].hours.Count; i++)
            {
                DateTime updated = date.AddHours(i);
                predictFormatTest tmp;
                tmp.cislo = predictNewWay(updated, i, native_wattpeek, native_azimuth, native_tilt);
                tmp.datum = i+"";
                Coordinate c = new Coordinate();
                Coordinate.TryParse("N"+Latitude+ "E"+Longitude, updated, out c);

                // var position= SunPosition.CalculateSunPosition(updated, latitude: double.Parse(Latitude, System.Globalization.CultureInfo.InvariantCulture), longitude: double.Parse(Longitude, System.Globalization.CultureInfo.InvariantCulture));
                tmp.azimuth = c.CelestialInfo.SunAzimuth;
                tmp.altitude = c.CelestialInfo.SunAltitude;
                tmp.solarenrgy = forecast7Days.days[0].hours[i].solarenergy * 1000;
                tmp.mraky = forecast7Days.days[0].hours[i].cloudcover;
                tmp.osvit = (Math.Sin(native_tilt) * Math.Cos(tmp.altitude) * Math.Cos(170 - tmp.azimuth)) + (Math.Cos(native_tilt) * Math.Sin(tmp.altitude));
                list.Add(tmp);
            }
            return list; 
        }
        private double predictNewWay(DateTime time, int hour, double wattpeak,double panel_az,double panel_tilt)
        {
            string sunrise = forecast7Days.days[0].sunrise;
            string sunset = forecast7Days.days[0].sunset;
            String[] strlistrise = sunrise.Split(":");
            String[] strlistset = sunset.Split(":");
            int hodinarise = Int32.Parse(strlistrise[0]);
            int hodinaset = Int32.Parse(strlistset[0]);

            var position = SunPosition.CalculateSunPosition(time, latitude: double.Parse(Latitude, System.Globalization.CultureInfo.InvariantCulture), longitude: double.Parse(Longitude, System.Globalization.CultureInfo.InvariantCulture));
            //double zakr = 1.0 - ((forecast7Days.days[0].hours[hour].cloudcover) / 100);
            double vysledek = (Math.Sin(panel_tilt) * Math.Cos(Math.Abs(position.Altitude)) * Math.Cos(panel_az - position.Azimuth) + (Math.Cos(panel_tilt) * Math.Sin(Math.Abs(position.Altitude))));
            double watt = forecast7Days.days[0].hours[hour].solarenergy * 1000;
            if (watt!=0 || watt>0)
            {
                if ((watt / 100) * 15 > wattpeak)
                {
                    return Math.Abs(vysledek)  * wattpeak;
                }
                else
                {
                    return Math.Abs(vysledek) * ((watt / 100) * 15);
                }
            }
            else
            {
                return 0;
            }
            
        }

    }
    class Program
    {

       
        static void Main(string[] args)
        {
            Start news = new Start();
            
        }

        

    }
}
