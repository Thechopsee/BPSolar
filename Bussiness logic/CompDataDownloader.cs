using BakalarskaPrace1.Bussiness_logic.forecast.valApi;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Bussiness_logic
{
    public class CompDataDownloader
    {
        
        public static List<predictionFormat> getdatafromValapi()
        {
            List<predictionFormat> data = new List<predictionFormat>();
            //http://10.0.0.91:2000/solar/apii/
            //http://85.162.168.74:7799/solar/apii/
            var client = new RestClient(@"http://85.162.168.74:7799/solar/apii/");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string res = response.Content.ToString();
            
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(res);

            DateTime dt = DateTime.Now;
            

            for (int i = 0; i < 7; i++)
            {
                double win=myDeserializedClass.data[i].pv_voltage * myDeserializedClass.data[i].batt_charging;
                predictionFormat temp;
                temp.cislo = win;
                temp.datum = i.ToString();
                data.Add(temp);
            }
            List<predictionFormat> full = new List<predictionFormat>();
            List<predictionFormat> empty = new List<predictionFormat>();
            
            for (int i = 0; i < data.Count; i++)
            {
                if ((int)dt.DayOfWeek - 1 == -1)
                {
                    predictionFormat temp;
                    temp.cislo = data[i].cislo;
                    
                    temp.datum = (7 - i) + "";
                    full.Add(temp);
                }
                else
                {
                    if (i > (int)dt.DayOfWeek - 1)
                    {

                        predictionFormat temp;
                        temp.cislo = 0;
                        temp.datum = (7 - i) + "";
                        empty.Add(temp);
                    }
                    else
                    {

                        predictionFormat temp;
                        temp.cislo = data[i].cislo;
                       
                        temp.datum = (7 - i) + "";
                        full.Add(temp);
                    }
                }
                
            }
            full.Reverse();
            foreach (predictionFormat x in empty)
            {
                full.Add(x);
            }
            return full;
        }
        public static List<predictionFormat> getDatafromForacestSolar()
        {
            List<predictionFormat> data = new List<predictionFormat>();
            var client = new RestClient(@"https://api.forecast.solar/estimate/49.754683/018.61115/66/-10/0.095");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string res = response.Content.ToString();
            
            string[] subs = res.Split('{');
            string[] subs2 = subs[3].Split('}');
            string[] prvky = subs2[0].Split(',');
            string datum=prvky[0].Split('"')[1];
            int count=0;
            int day=0;
            try
            {
                DateTime dt = Convert.ToDateTime(datum);
                count = dt.Hour;
                day = dt.Day;
            }
            catch (FormatException e)
            {
                for (int i = 1; i < 24; i++)
                {
                    
                    predictionFormat temp;
                    temp.cislo = 0;
                    temp.datum = i.ToString();
                    data.Add(temp);
                }
                return data;
            }
            predictionFormat tempP;
            tempP.cislo = 0;
            tempP.datum = "0";
            data.Add(tempP);
            for (int i = 0; i < count; i++)
            {
                predictionFormat temp;
                temp.cislo =0;
                temp.datum = i.ToString();
                data.Add(temp);
            }
            predictionFormat tmp;
            
            tmp.cislo = Convert.ToInt32(prvky[0].Split('"')[2].Split(':')[1]);
            tmp.datum = count.ToString();
            data.Add(tmp);
            int counter2 = 0;
            int kolikmin = 0;
            foreach (string x in prvky)
            {
                string new_DATE = x.Split('"')[1];
                string dat = x.Split('"')[1];
                DateTime dtt = Convert.ToDateTime(new_DATE);
                
                if (dtt.Day != day)
                {
                    break;
                }
                if (dtt.Hour == count)
                {
                    kolikmin++;
                    continue;
                }
                else
                {
                    
                    double num = Convert.ToInt32(x.Split('"')[2].Split(',')[0].Split(':')[1]);
                    predictionFormat temp;
                    temp.cislo = num;
                    temp.datum = dtt.Hour.ToString();
                    data.Add(temp);
                    counter2++;
                }
            }
            for (int i = data.Count; i < 24; i++)
            {
                predictionFormat temp;
                temp.cislo = 0;
                temp.datum = i.ToString();
                data.Add(temp);
            }
            return data;
        }
    }
}
