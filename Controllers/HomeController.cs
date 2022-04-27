using api_downloads.ForecastVissualCrossing;
using BakalarskaPrace1.Bussiness_logic;
using BakalarskaPrace1.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace BakalarskaPrace1.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LocationM lokace)
        {
            return RedirectToAction("Location");
        }
        public IActionResult Location()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Location(LocationM lokace)
        {
            bool validateAllProperties = false;

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                lokace,
                new ValidationContext(lokace, null, null),
                results,
                validateAllProperties);
            ViewBag.watopen = 0;
            if (!isValid)
            {
                string result_string = "";
                foreach (ValidationResult x in results)
                {
                    result_string += x.ToString();
                }
                ViewBag.watopen = 1;
                ViewBag.mistakes = result_string;
                return View();
            }
            return RedirectToAction("Panel",lokace);
        }

        public IActionResult Panel(LocationM lokace)
        {
            
            if (!String.IsNullOrEmpty(lokace.mesto) && !String.IsNullOrEmpty(lokace.ISO))
            {
                ViewBag.mesto = lokace.mesto;
                ViewBag.iso = lokace.ISO;
            }
            else if (!String.IsNullOrEmpty(lokace.latitude) && !String.IsNullOrEmpty(lokace.longitude))
            {
                ViewBag.latitude = lokace.latitude;
                ViewBag.longitude = lokace.longitude;
            }

            return View();
        }
        [HttpPost]
        public IActionResult Panel(PowerPlantM form)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Solar", form);
            }
            else
            {
                if (!String.IsNullOrEmpty(form.mesto) && !String.IsNullOrEmpty(form.ISO))
                {
                    ViewBag.mesto = form.mesto;
                    ViewBag.iso = form.ISO;
                }
                else if (!String.IsNullOrEmpty(form.latitude) && !String.IsNullOrEmpty(form.longitude))
                {
                    ViewBag.latitude = form.latitude;
                    ViewBag.longitude = form.longitude;
                }
                return View();
            }
        }

        public IActionResult Solar(PowerPlantM form)
        {
            SolarForecaster solarforecaster = new SolarForecaster();

            if (ModelState.IsValid)
            {
                ViewBag.wattpeak = form.wattpeek;
                ViewBag.tilt = form.panel_tilt;
                ViewBag.azimut = form.panel_azimuth;
                solarforecaster.HledanaElektrarna = form;

                solarforecaster.updateFullData();
                VisForecast vf=solarforecaster.getFullForecast();
                ViewBag.cloud = vf.currentConditions.cloudcover;
                ViewBag.conditions = vf.currentConditions.conditions;
                ViewBag.rain = vf.currentConditions.precip;
                ViewBag.solar = vf.currentConditions.solarenergy * 1000;
                if (DateTime.Now.Hour < 23 && DateTime.Now.Hour>0)
                {
                    ViewBag.rain31 = vf.days[0].hours[DateTime.Now.Hour - 1].precip;
                    ViewBag.rain32 = vf.days[0].hours[DateTime.Now.Hour].precip;
                    ViewBag.rain33 = vf.days[0].hours[DateTime.Now.Hour + 1].precip;
                    ViewBag.cloud31 = vf.days[0].hours[DateTime.Now.Hour - 1].cloudcover;
                    ViewBag.cloud32 = vf.days[0].hours[DateTime.Now.Hour].cloudcover;
                    ViewBag.cloud33 = vf.days[0].hours[DateTime.Now.Hour + 1].cloudcover;
                    ViewBag.energy31 = vf.days[0].hours[DateTime.Now.Hour - 1].solarenergy * 1000;
                    ViewBag.energy32 = vf.days[0].hours[DateTime.Now.Hour].solarenergy * 1000;
                    ViewBag.energy33 = vf.days[0].hours[DateTime.Now.Hour + 1].solarenergy * 1000;
                }
                else if(DateTime.Now.Hour!= 0)
                {
                    ViewBag.rain31 = vf.days[0].hours[DateTime.Now.Hour - 1].precip;
                    ViewBag.rain32 = vf.days[0].hours[DateTime.Now.Hour].precip;
                    ViewBag.rain33 = vf.days[1].hours[0].precip;
                    ViewBag.cloud31 = vf.days[0].hours[DateTime.Now.Hour - 1].cloudcover;
                    ViewBag.cloud32 = vf.days[0].hours[DateTime.Now.Hour].cloudcover;
                    ViewBag.cloud33 = vf.days[1].hours[0].cloudcover;
                    ViewBag.energy31 = vf.days[0].hours[DateTime.Now.Hour - 1].solarenergy * 1000;
                    ViewBag.energy32 = vf.days[0].hours[DateTime.Now.Hour].solarenergy * 1000;
                    ViewBag.energy33 = vf.days[1].hours[0].solarenergy * 1000;
                }
                if (DateTime.Now.Hour == 0)
                {
                    ViewBag.rain31 = vf.days[0].hours[0].precip;
                    ViewBag.rain32 = vf.days[0].hours[DateTime.Now.Hour].precip;
                    ViewBag.rain33 = vf.days[0].hours[DateTime.Now.Hour+1].precip;
                    ViewBag.cloud31 = vf.days[0].hours[0].cloudcover;
                    ViewBag.cloud32 = vf.days[0].hours[DateTime.Now.Hour].cloudcover;
                    ViewBag.cloud33 = vf.days[0].hours[DateTime.Now.Hour+1].cloudcover;
                    ViewBag.energy31 = vf.days[0].hours[0].solarenergy * 1000;
                    ViewBag.energy32 = vf.days[0].hours[DateTime.Now.Hour].solarenergy * 1000;
                    ViewBag.energy33 = vf.days[0].hours[DateTime.Now.Hour+1].solarenergy * 1000;
                }
                string sp7 = "[";
                List<SelectedPrediction> lsp = new List<SelectedPrediction>();
                double cloudcount = 0;
                double suncount = 0;
                double raincount = 0;
                for (int i = 0; i < 7; i++)
                {
                    SelectedPrediction sp = new SelectedPrediction();
                        var cc = vf.days[i].cloudcover;
                         cloudcount += cc;   
                        var energy = vf.days[i].solarenergy;
                    suncount += energy;
                        var rain = vf.days[i].precip;
                    raincount += rain;
                    lsp.Add(sp);
                    sp.clouds = cc;
                    sp.sun = energy;
                    sp.rain = rain;
                    string json7d = System.Text.Json.JsonSerializer.Serialize(sp);
                    sp7 += json7d;
                    if (i < 6)
                    {
                        sp7 += ",";
                    }
                }
                
                ViewBag.weekprumersun = String.Format("{0:0.00}", suncount / 7);
                ViewBag.weekprumercloud = String.Format("{0:0.00}", cloudcount / 7);
                ViewBag.weekprumerrain = String.Format("{0:0.00}", raincount / 7);


                ViewBag.Weather7day = new HtmlString(sp7+ "]");
                solarforecaster.createPrediction(predictionEnum.THRS);
                solarforecaster.createPrediction(predictionEnum.SDYS);
                solarforecaster.createPrediction(predictionEnum.WEEK);
                string predpoved7 = GraphPreparer.PrepareData2Cols(solarforecaster.get7dysPrediction(),"Days");
                List<string> predpoveddbd = GraphPreparer.PrepareData7(solarforecaster.getWeekPrediction());
                string predpoved3 = GraphPreparer.PrepareData2Cols(solarforecaster.get3hrsPrediction(),"Hour");
                
                ViewBag.Data = new HtmlString(predpoved7);
                ViewBag.day1 = new HtmlString(predpoveddbd[0]);
                ViewBag.day2 = new HtmlString(predpoveddbd[1]);
                ViewBag.day3 = new HtmlString(predpoveddbd[2]);
                ViewBag.day4 = new HtmlString(predpoveddbd[3]);
                ViewBag.day5 = new HtmlString(predpoveddbd[4]);
                ViewBag.day6 = new HtmlString(predpoveddbd[5]);
                ViewBag.day7 = new HtmlString(predpoveddbd[6]);
                ViewBag.alldays = new HtmlString(predpoveddbd[7]);

                ViewBag.hours3 = new HtmlString(predpoved3);

                
                string dates = "[";
                foreach (Day x in vf.days)
                {
                    if (!String.Equals(dates, "["))
                    {
                        dates += ",";
                    }
                    dates += $@"""{x.datetime}""";
                }
                dates += "]";
                ViewBag.dates = new HtmlString(dates);
                ViewBag.city = vf.address;

                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult API()
        {
            return View();
        }
        public IActionResult Comparation()
        {
            SolarForecaster solarforecaster = new SolarForecaster();
            solarforecaster.HledanaElektrarna = solarforecaster.testovaciElektr;

            solarforecaster.updateFullData();
            solarforecaster.createPrediction(predictionEnum.THRS);
            solarforecaster.createPrediction(predictionEnum.SDYS);
            solarforecaster.createPrediction(predictionEnum.WEEK);

            DatabaseHandler dh = new DatabaseHandler();
            List<List<predictionFormat>> list = solarforecaster.getWeekPrediction();

            List<predictionFormat> ls = list[0];
            
            VisForecast vs=solarforecaster.getFullForecast();
            /*foreach (api_downloads.ForecastVissualCrossing.Hour x in vs.days[0].hours)
            {
                Console.WriteLine("cc:"+x.cloudcover);
                Console.WriteLine("se:" + x.solarenergy);
            }
            Console.WriteLine("moje:");
            foreach (predictionFormat x in ls)
            {
                Console.WriteLine(x.cislo);
            }*/

            List<predictionFormat> cizi = CompDataDownloader.getDatafromForacestSolar();

            string graf = GraphPreparer.prepareSrovnani(ls, cizi);
            ViewBag.alldays = graf;
            
            List<predictionFormat> cizi2=CompDataDownloader.getdatafromValapi();
            string graf2 = GraphPreparer.prepareSrovnani(dh.getData7dbd(), cizi2,true);
            ViewBag.graf2_data = graf2;
            return View();
        }
    }
}
