using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace TestProject1
{
    internal class webtest
    {

        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        [TestCase("-90.0000000", "-180.0000000", "-180","0","1")]
        [TestCase("-85.0000000", "-150.0000000", "-150", "10", "2")]
        [TestCase("0.0000000", "0.0000000", "0", "45", "1000")]
        [TestCase("85.0000000", "150.0000000", "150", "80", "9999")]
        [TestCase("90.0000000", "180.0000000", "180", "90", "10000")]
        public void testSimple(string lat,string lon,string azimuth,string tilt,string panel)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AcceptInsecureCertificates = true;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("https://localhost:44321");
            //var element = driver.FindElement(By.Id("categoriesContent")).FindElement(By.PartialLinkText("Hardware"));
            //element.Click();
            var el = driver.FindElement(By.Id("smf"));
            el.Click();
            var title = driver.FindElement(By.Id("main_text")).Text;
            Assert.AreEqual("Select Location:", title);
            var el2 = driver.FindElement(By.Id("button_geo"));
            el2.Click();
            var el_lat = driver.FindElement(By.Id("latitude"));
            el_lat.SendKeys(lat);
            var el_lot = driver.FindElement(By.Id("longitude"));
            el_lot.SendKeys(lon);

            var el3 = driver.FindElement(By.Id("ukaz"));
            el3.Click();
            title = driver.FindElement(By.Id("main_text")).Text;
            Assert.AreEqual("Power plant properties:", title);

            var panel_tilt = driver.FindElement(By.Id("panel_tilt"));
            panel_tilt.SendKeys(tilt);
            var panel_azimuth = driver.FindElement(By.Id("panel_azimuth"));
            panel_azimuth.SendKeys(azimuth);
            var wattpeek = driver.FindElement(By.Id("wattpeek"));
            wattpeek.SendKeys(panel);
            var el4 = driver.FindElement(By.Id("dal"));
            el4.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Id("double_info_box")));
        }
        [Test]
        [TestCase(-90.0000000, 185.0000000, -180, 0, 1)]
        [TestCase(-90.0000000, -180.0000000, 185, 0, 1)]
        [TestCase(-90.0000000, -180.0000000, -180, 100, 1)]
        [TestCase(-90.0000000, -180.0000000, -180, 0, 11111)]
        [TestCase(95.0000000, -180.0000000, -180, 0, 1)]
        [Parallelizable(ParallelScope.All)]
        public void testFalseUp(double lat, double lon, double azimuth, double tilt, double panel)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AcceptInsecureCertificates = true;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("https://localhost:44321");
            
            var el = driver.FindElement(By.Id("smf"));
            el.Click();
            var title = driver.FindElement(By.Id("main_text")).Text;
            Assert.AreEqual("Select Location:", title);
            var el2 = driver.FindElement(By.Id("button_geo"));
            el2.Click();
            var el_lat = driver.FindElement(By.Id("latitude"));
            el_lat.SendKeys(lat+"");
            var el_lot = driver.FindElement(By.Id("longitude"));
            el_lot.SendKeys(lon+"");

            var el3 = driver.FindElement(By.Id("ukaz"));
            el3.Click();
            if (lat > 90)
            {
                var warning = driver.FindElement(By.Id("Warning2"));
                Assert.AreEqual("Latitude is out of range", warning.Text);
                return;

            }
            else if (lon > 180)
            {
                var warning = driver.FindElement(By.Id("Warning2"));
                Assert.AreEqual("Longitude is out of range", warning.Text);
                return;
                
            }
            var panel_tilt = driver.FindElement(By.Id("panel_tilt"));
            panel_tilt.SendKeys(tilt + "") ;
            var panel_azimuth = driver.FindElement(By.Id("panel_azimuth"));
            panel_azimuth.SendKeys(azimuth + "");
            var wattpeek = driver.FindElement(By.Id("wattpeek"));
            wattpeek.SendKeys(panel + "");
            
            var el4 = driver.FindElement(By.Id("dal"));
            el4.Click();
            if (azimuth > 180)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field panel_azimuth must be between -180 and 180.", warning.Text);
            }
            else if (tilt > 90)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field panel_tilt must be between 0 and 90.", warning.Text);
            }
            else if (panel > 10000)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field wattpeek must be between 0 and 10000.", warning.Text);
            }

        }
        [Test]
        [TestCase(-90.0000000, -185.0000000, -180, 0, 1)]
        [TestCase(-90.0000000, -180.0000000, -185, 0, 1)]
        [TestCase(-90.0000000, -180.0000000, -180, -1, 1)]
        [TestCase(-90.0000000, -180.0000000, -180, 0, -1)]
        [TestCase(-95.0000000, -180.0000000, -180, 0, 1)]
        [Parallelizable(ParallelScope.All)]
        public void testFalseDown(double lat, double lon, double azimuth, double tilt, double panel)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AcceptInsecureCertificates = true;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("https://localhost:44321");

            var el = driver.FindElement(By.Id("smf"));
            el.Click();
            var title = driver.FindElement(By.Id("main_text")).Text;
            Assert.AreEqual("Select Location:", title);
            var el2 = driver.FindElement(By.Id("button_geo"));
            el2.Click();
            var el_lat = driver.FindElement(By.Id("latitude"));
            el_lat.SendKeys(lat + "");
            var el_lot = driver.FindElement(By.Id("longitude"));
            el_lot.SendKeys(lon + "");

            var el3 = driver.FindElement(By.Id("ukaz"));
            el3.Click();
            if (lat < -90)
            {
                var warning = driver.FindElement(By.Id("Warning2"));
                Assert.AreEqual("Latitude is out of range", warning.Text);
                return;

            }
            else if (lon < -180)
            {
                var warning = driver.FindElement(By.Id("Warning2"));
                Assert.AreEqual("Longitude is out of range", warning.Text);
                return;

            }
            var panel_tilt = driver.FindElement(By.Id("panel_tilt"));
            panel_tilt.SendKeys(tilt + "");
            var panel_azimuth = driver.FindElement(By.Id("panel_azimuth"));
            panel_azimuth.SendKeys(azimuth + "");
            var wattpeek = driver.FindElement(By.Id("wattpeek"));
            wattpeek.SendKeys(panel + "");

            var el4 = driver.FindElement(By.Id("dal"));
            el4.Click();
            if (azimuth > 180)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field panel_azimuth must be between -180 and 180.", warning.Text);
            }
            else if (tilt > 90)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field panel_tilt must be between 0 and 90.", warning.Text);
            }
            else if (panel < 0)
            {
                var warning = driver.FindElement(By.Id("validation"));
                Assert.AreEqual("The field wattpeek must be between 0 and 10000.", warning.Text);
            }

        }

    }
}
