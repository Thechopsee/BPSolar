using BakalarskaPrace1.Models;
using System.Collections.Generic;

namespace BakalarskaPrace1.IServices
{
    public interface IWeatherService
    {
        string GetData();
        string Get3HourForecast(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude);
        string Get7DaysForecast(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude);
        string GetWeekForecast(double panel_tilt, double panel_azimuth, double wattpeek, string longitude, string latitude);

    }
}