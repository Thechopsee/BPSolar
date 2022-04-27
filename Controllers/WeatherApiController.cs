using BakalarskaPrace1.Bussiness_logic;
using BakalarskaPrace1.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherApiController : ControllerBase
    {
        private readonly IWeatherService weatherservice;

        public WeatherApiController(IWeatherService weather)
        {
            weatherservice = weather;
        }

        private string inputValidator(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude)
        {
            string output = " ";
            if (panel_tilt == null || panel_azimuth == null || wattpeek == null || latitude == null || longitude == null)
            {
                output += "lack of data,";
            }
            if (panel_tilt < 0 || panel_tilt > 90)
            {
                output += "bad panel tilt,";
            }
            if (panel_azimuth < -180 || panel_azimuth > 180)
            {
                output += "azimuth of panel out of range,";
            }
            if (Double.Parse(latitude, System.Globalization.CultureInfo.InvariantCulture) < -90 || Double.Parse(latitude, System.Globalization.CultureInfo.InvariantCulture) > 90 || Double.Parse(longitude, System.Globalization.CultureInfo.InvariantCulture) < -180 || Double.Parse(longitude, System.Globalization.CultureInfo.InvariantCulture) > 180)
            {
                output += "geolocation cords out of range,";
            }
            if (wattpeek<0)
            {
                output += "wattpeek is out of range";
            }
            return output;
        }
        [HttpGet]
        [Route("/api")]
        public string GetData()
        {
            return weatherservice.GetData();
        }
        [HttpGet]
        [Route("/api/3HRS")]
        public string get3Hours(double panel_tilt,double panel_azimuth,double wattpeek,string longitude, string latitude)
        {
            string validationOutput=inputValidator(panel_tilt,panel_azimuth,wattpeek,longitude,latitude);
            if (validationOutput.Length > 2)
            {
                RedirectToAction("APIError", new { error_msg = validationOutput }); 
            }
            return weatherservice.Get3HourForecast(panel_tilt,panel_azimuth,wattpeek,longitude, latitude);
        }
        [HttpGet]
        [Route("/api/7DYS")]
        public string get7days(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude)
        {
           return weatherservice.Get7DaysForecast(panel_tilt, panel_azimuth, wattpeek, longitude, latitude);
        }

        [HttpGet]
        [Route("/api/WEEK")]
        public string getWeek(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude)
        {
            string validationOutput = inputValidator(panel_tilt, panel_azimuth, wattpeek, longitude, latitude);
            if (validationOutput.Length > 2)
            {
                RedirectToAction("APIError", new { error_msg = validationOutput });
            }
            return weatherservice.GetWeekForecast(panel_tilt, panel_azimuth, wattpeek, longitude, latitude);
        }

        [HttpGet]
        [Route("/api/saveForecast")]
        public string save(string key)
        {
            string trukey = "hello";
            if (String.Equals(trukey,key))
            {
                DatabaseHandler fdh = new DatabaseHandler();
                SolarForecaster sf = new SolarForecaster();
                sf.HledanaElektrarna = sf.testovaciElektr;
                sf.updateFullData(false);
                sf.createPrediction(predictionEnum.SDYS);
                List<predictionFormat> pred = sf.get7dysPrediction();
                fdh.Savetodb(pred);
                return "updated";
            }
            return "bad password";
        }
        public string APIError(string error_msg)
        {
            return error_msg;
        }
    }
}
