using BakalarskaPrace1.Bussiness_logic;
using BakalarskaPrace1.Controllers;
using BakalarskaPrace1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.IServices
{
    public class WeatherService : IWeatherService
    {
        public WeatherService() {

        }
        public string GetData()
        {

            return "hello world";
        }
        public string Get3HourForecast(double panel_tilt,double panel_azimuth,double wattpeek,string longitude, string latitude)
        {
            SolarForecaster sf = new SolarForecaster();
            PowerPlantM temp = new PowerPlantM();
            temp.panel_azimuth = panel_azimuth;
            temp.panel_tilt = panel_tilt;
            temp.wattpeek = wattpeek;
            temp.longitude = longitude;
            temp.latitude = latitude;
            sf.HledanaElektrarna = temp;
            sf.updateFullData();
            sf.createPrediction(predictionEnum.THRS);
           // sf.updateLocation(longitude, latitude, true);
            List<predictionFormat> data = sf.get3hrsPrediction();
            ApiStructure3hrs A = new ApiStructure3hrs();
            for (int i = 0; i < 3; i++)
            {
                Hour hour;
                hour.hour_num = data[i].datum;
                hour.powerOutput = data[i].cislo;
                A.hours.Add(hour);
            }
         
            return JsonConvert.SerializeObject(A);
        }

        public string Get7DaysForecast(double panel_tilt,double panel_azimuth,double wattpeek,string longitude, string latitude)
        {
            
            SolarForecaster sf = new SolarForecaster();
            PowerPlantM tempp = new PowerPlantM();
            tempp.panel_azimuth = panel_azimuth;
            tempp.panel_tilt = panel_tilt;
            tempp.wattpeek = wattpeek;
            tempp.longitude = longitude;
            tempp.latitude = latitude;
            sf.HledanaElektrarna = tempp;
            sf.updateFullData();
            sf.createPrediction(predictionEnum.SDYS);
            //sf.updateLocation(longitude, latitude, true);
            List<predictionFormat> data = sf.get7dysPrediction();
            ApiStructureForForecast A = new ApiStructureForForecast();
            A.latitude = sf.getLatitude();
            A.longtitude = sf.getLongitude();
            for (int i = 0; i < 7; i++)
            {
                Days temp = new Days();
                temp.date = (DateTime.Now.Date.AddDays(i)).ToShortDateString();
                temp.powerOutput = data[i].cislo;
                A.days.Add(temp);
            }
            
            return JsonConvert.SerializeObject(A, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

        }

        public string GetWeekForecast(double panel_tilt,double panel_azimuth,double wattpeek,string longitude, string latitude)
        {
            
            SolarForecaster sf = new SolarForecaster();
           
            
            PowerPlantM tempp = new PowerPlantM();
            tempp.panel_azimuth = panel_azimuth;
            tempp.panel_tilt = panel_tilt;
            tempp.wattpeek = wattpeek;
            tempp.longitude = longitude;
            tempp.latitude = latitude;
            sf.HledanaElektrarna = tempp;
            sf.updateFullData();
            sf.createPrediction(predictionEnum.SDYS);
            List<List<predictionFormat>> data7 = sf.getWeekPrediction();
            ApiStructureForForecast A = new ApiStructureForForecast();
            A.latitude = sf.getLatitude();
            A.longtitude = sf.getLongitude();
            for (int i = 0; i < 7; i++)
            {
                Days temp = new Days();
                temp.hours = new List<Hour>(); 
                temp.date = (DateTime.Now.Date.AddDays(i)).ToShortDateString();
                double soucet = 0;
                double power_prm = 0;
                int delitel = 0;
                for (int j = 0; j < 24; j++)
                {
                    Hour tmp = new Hour();
                    tmp.hour_num = j.ToString();
                    tmp.powerOutput = data7[i][j].cislo;
                    if (tmp.powerOutput != 0)
                    {
                        delitel++;
                        power_prm += tmp.powerOutput;
                    }
                   
                    temp.hours.Add(tmp);
                    soucet += data7[i][j].cislo;

                }

                temp.powerOutput = power_prm/delitel;

                A.days.Add(temp);
            }
            
            return JsonConvert.SerializeObject(A);


        }
    }

    
}
