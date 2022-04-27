using api_downloads.ForecastVissualCrossing;

using BakalarskaPrace1.Models;
using CoordinateSharp;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Bussiness_logic
{
    public struct predictionFormat
    {
        public double cislo;
        public string datum;
       
    };
    public enum predictionEnum
    {
        SDYS,
        THRS,
        WEEK,

    }

    public class SolarForecaster
    {
        string Longitude;
        string Latitude;
        VisForecast forecast7Days;
        public PowerPlantM testovaciElektr;
        public PowerPlantM HledanaElektrarna;
        List<List<predictionFormat>> WEEKprediction;
        List<predictionFormat> SDYSprumer;
        List<predictionFormat> THRSprediction;

        public SolarForecaster()
        {
            WEEKprediction = new List<List<predictionFormat>>();
            testovaciElektr = new PowerPlantM();
            testovaciElektr.latitude = "49.754683";
            testovaciElektr.longitude = "18.61115";
            testovaciElektr.panel_tilt = 66;
            testovaciElektr.panel_azimuth = -10;
            testovaciElektr.wattpeek = 95;
            
        }
        public string getLatitude()
        {
            return this.Latitude;
        }
        public string getLongitude()
        {
            return this.Longitude;
        }
        public VisForecast getFullForecast()
        {
            return this.forecast7Days;
        }
        public List<predictionFormat> get7dysPrediction()
        {
            return this.SDYSprumer;
        }
        public List<predictionFormat> get3hrsPrediction()
        {
            return this.THRSprediction;
        }
        public List<List<predictionFormat>> getWeekPrediction()
        {
            return this.WEEKprediction;
        }

        
        public void createPrediction(predictionEnum typ)
        {
            
            if (HledanaElektrarna == null)
            {
                return;
            }
            if (typ == predictionEnum.WEEK)
            {
                List<List<predictionFormat>> pf = new List<List<predictionFormat>>();
                for (int i = 0; i < 7; i++)
                {
                    pf.Add(ListcreatePrediction(24,i));
                }
                this.WEEKprediction = pf;
            }
            else if (typ == predictionEnum.SDYS)
            {
                if (this.WEEKprediction.Count == 0)
                {
                    createPrediction(predictionEnum.WEEK);
                }
                List<predictionFormat> prumery = new List<predictionFormat>();
                int dc = 1;
                foreach (List<predictionFormat> x in WEEKprediction)
                {
                    double vysledky = 0;
                    int count = 0;
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (x[i].cislo > 0)
                        {
                            vysledky+=x[i].cislo;
                            count++;
                        }
                    }
                    predictionFormat temp;
                    temp.cislo = vysledky / count;
                    temp.datum = dc+"";
                    prumery.Add(temp);
                    dc++;
                }
                this.SDYSprumer = prumery;
            }
            else
            {
                
                this.THRSprediction = (ListcreatePrediction(3,0, DateTime.Now.Hour));
            }
           
        }
        public List<predictionFormat> ListcreatePrediction(int numOfHours,int starting_day=0,int startingHour=0)
        {
            int hour = startingHour;
            DateTime date = DateTime.Parse(forecast7Days.days[starting_day].datetime);

            List<double> solar = new List<double>();
            int day = starting_day;

            for (int i = 0; i < numOfHours; i++)
            {
                if (hour + i >= 24)
                {
                    hour -= 24;
                    date.AddDays(1);
                    day += 1;
                }
                Coordinate c = new Coordinate();
                CoordinatePart lat;
                CoordinatePart.TryParse("N"+ HledanaElektrarna.latitude , out lat);

                CoordinatePart lon;
                CoordinatePart.TryParse("E"+ HledanaElektrarna.longitude, out lon);
                c.Latitude = lat;
                c.Longitude = lon;

                c.GeoDate = (date).AddHours(hour + i);
                double zakr = 1.1 - ((forecast7Days.days[day].hours[hour + i].cloudcover) / 100);
                

                double vysledek = (Math.Sin(HledanaElektrarna.panel_tilt) * Math.Cos(c.CelestialInfo.SunAltitude) * Math.Cos(HledanaElektrarna.panel_azimuth - c.CelestialInfo.SunAzimuth)) + (Math.Cos(HledanaElektrarna.panel_tilt) * Math.Sin(c.CelestialInfo.SunAltitude));
               
                double watt = forecast7Days.days[day].hours[hour + i].solarenergy*1000;
                
                if (watt != 0 || watt > 0)
                {
                    if ((watt / 100) * 15 > HledanaElektrarna.wattpeek)
                    {
                        solar.Add(Math.Abs(vysledek) * zakr * HledanaElektrarna.wattpeek);
                    }
                    else
                    {
                        solar.Add(Math.Abs(vysledek) * zakr * ((watt / 100) * 15));
                    }
                }
                else
                {
                    solar.Add(0);
                }
            }
            List<predictionFormat> list = new List<predictionFormat>();
            int counter = 0;
            foreach (double x in solar)
            {
                predictionFormat temp = new predictionFormat();
                temp.datum = hour + counter + "";
                temp.cislo = x;
                list.Add(temp);
                counter++;

            }
            return list;
        }
        public bool updateFullData(bool city=false)
        {
            IRestResponse response;
            if (city != true)
            {
                response = getDatafromVCWeather(HledanaElektrarna.latitude, HledanaElektrarna.longitude);
            }
            else
            {
                response = getDatafromVCWeather(HledanaElektrarna.mesto+HledanaElektrarna.ISO);
            }
            this.forecast7Days = JsonConvert.DeserializeObject<VisForecast>(response.Content.ToString());
            this.Latitude=this.forecast7Days.latitude+"";
            this.Longitude = this.forecast7Days.longitude + "";

            return true;
        }
        public  IRestResponse getDatafromVCWeather(string location)
        {
            string start = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
            start += location;
            start +="?unitGroup=metric&key=NKHJXB5LAXMJGA7N6DV8BCVDL&options=nonulls";
            var client = new RestClient(start);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        static public IRestResponse getDatafromVCWeather(string lat,string lon)
        {
            string start = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
            start += lat+ "%2C" + lon;
            start += "?unitGroup=metric&key=NKHJXB5LAXMJGA7N6DV8BCVDL&options=nonulls";
           
            var client = new RestClient(start);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
       
    }
}
