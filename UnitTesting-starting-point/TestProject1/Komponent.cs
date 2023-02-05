using api_downloads.ForecastVissualCrossing;
using BakalarskaPrace1.Bussiness_logic;
using BakalarskaPrace1.Controllers;
using BakalarskaPrace1.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject1
{

    internal class Komponent
    {
        SolarForecaster solarforecaster;
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        [TestCase("-90.0000000", "-180.0000000", -180, 0, 1)]
        [TestCase("-85.0000000", "-150.0000000", -150, 10, 2)]
        [TestCase("0.0000000", "0.0000000", 0, 45, 1000)]
        [TestCase("85.0000000", "-180.0000000", 150, 80, 9999)]
        [TestCase("90.0000000", "180.0000000", 180, 90, 10000)]
        public void GeterseterTest(string lat, string lon, double azimuth, double tilt, double panel)
        {
            
            solarforecaster = new SolarForecaster();
            var moq = new Mock<PowerPlantM>();
            moq.Object.latitude = lat;
            moq.Object.longitude = lon;
            moq.Object.panel_azimuth = azimuth;
            moq.Object.panel_tilt = tilt;
            moq.Object.wattpeek = panel;
            solarforecaster.HledanaElektrarna = moq.Object;
            string latt=solarforecaster.getLatitude();
            string lonn=solarforecaster.getLongitude();
            Assert.AreEqual(latt, lat);
            Assert.AreEqual(lonn, lon);

        }
        [Test]
        [TestCase("-90.0000000", "-180.0000000", -180, 0, 1)]
        [TestCase("-85.0000000", "-150.0000000", -150, 10, 2)]
        [TestCase("0.0000000", "0.0000000", 0, 45, 1000)]
        [TestCase("85.0000000", "-180.0000000", 150, 80, 9999)]
        [TestCase("90.0000000", "179.0000000", 180, 90, 1000)]
        public void APIgettest(string lat, string lon, double azimuth, double tilt, double panel)
        {
            solarforecaster= new SolarForecaster();
            var moq = new Mock<PowerPlantM>();
            moq.Object.latitude = lat;
            moq.Object.longitude = lon;
            moq.Object.panel_azimuth = azimuth;
            moq.Object.panel_tilt = tilt;
            moq.Object.wattpeek = panel;
            solarforecaster.HledanaElektrarna = moq.Object;
            solarforecaster.updateFullData();
            VisForecast vf=solarforecaster.getFullForecast();
            double latt=vf.latitude;
            double lonn=vf.longitude;
            Assert.AreEqual(latt.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture), lat);
            Assert.AreEqual(lonn.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture), lon);

        }
        [Test]
        [TestCase("-90.0000000", "-180.0000000", -180, 0, 1)]
        [TestCase("-85.0000000", "-150.0000000", -150, 10, 2)]
        [TestCase("0.0000000", "0.0000000", 0, 45, 1000)]
        [TestCase("85.0000000", "-180.0000000", 150, 80, 9999)]
        [TestCase("90.0000000", "179.0000000", 180, 90, 10000)]
        public void KomponentTest(string lat, string lon,double  azimuth,double tilt, double panel)
        {
            solarforecaster = new SolarForecaster();
            var moq = new Mock<PowerPlantM>();
            moq.Object.latitude = lat;
            moq.Object.longitude = lon;
            moq.Object.panel_azimuth = azimuth;
            moq.Object.panel_tilt = tilt;
            moq.Object.wattpeek = panel;
            solarforecaster.HledanaElektrarna = moq.Object;

            solarforecaster.updateFullData();
            solarforecaster.createPrediction(predictionEnum.THRS);
            solarforecaster.createPrediction(predictionEnum.SDYS);
            solarforecaster.createPrediction(predictionEnum.WEEK);
            solarforecaster.get3hrsPrediction();
            solarforecaster.get7dysPrediction();
            solarforecaster.getWeekPrediction();
            

        }

    }
}
