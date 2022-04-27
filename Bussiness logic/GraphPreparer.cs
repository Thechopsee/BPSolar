using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Bussiness_logic
{
    public class GraphPreparer
    {
        private static string prepareAllDays(List<List<predictionFormat>> list)
        {
            string dataStr = @"[[""Hour"",""Day1"",""Day2"",""Day3"",""Day4"",""Day5"",""Day6"",""Day7""]";
            string druhapulka = "";
            for (int i = 0; i < 24; i++)
            {
                druhapulka = druhapulka + $@",[{i},{Math.Round(list[0][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[1][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[2][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[3][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[4][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[5][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(list[6][i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
            }

            druhapulka = druhapulka + "]";

            dataStr += druhapulka;

            return dataStr;
        }
        public static String prepareSrovnani(List<predictionFormat> moje, List<predictionFormat> cizi,bool seven=false)
        {

            String dataStr;
            string druhapulka = "";
            if (!seven)
            {
                dataStr= @"[[""Hour"",""My prediction"",""Forecast.solar prediction"" ]";
                for (int i = 0; i < 24; i++)
                {
                    druhapulka = druhapulka + $@",[{moje[i].datum},{Math.Round(moje[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(cizi[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
                }
            }
            else
            {
                bool first = true;
                
                dataStr = @"[[""Hour"",""My prediction"",""Real power output""]";
                for (int i = 0; i < 7; i++)
                {
                    if (moje[i].cislo > 0)
                    {
                        druhapulka = druhapulka + $@",[{moje[i].datum},{Math.Round(moje[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(cizi[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
                    }
                    else if(first)
                    {
                        druhapulka = druhapulka + $@",[{moje[i].datum},{Math.Round(moje[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)},{Math.Round(cizi[i].cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
                        first = false;
                    }
                }
            }
            druhapulka = druhapulka + "]";
            dataStr += druhapulka;
            return dataStr;
        }
        public static string PrepareData2Cols(List<predictionFormat> list,string col1)
        {
            string dataStr = @$"[[""{col1}"",""PowerOutcome""]";
            string druhapulka = "";
            foreach (predictionFormat x in list)
            {
                druhapulka = druhapulka + $@",[{x.datum},{Math.Round(x.cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
            }

            druhapulka = druhapulka + "]";

            dataStr += druhapulka;


            return dataStr;
        }
        public static List<string> PrepareData7(List<List<predictionFormat>> data)
        {
            
            List<string> prepared=new List<string>();
            foreach (List<predictionFormat> solarday in data)
            {
                List<predictionFormat> list = new List<predictionFormat>();
                int counter = 0;
                foreach (predictionFormat x in solarday)
                {
                    predictionFormat temp = new predictionFormat();
                    temp.datum = counter + "";
                    temp.cislo = x.cislo;
                    list.Add(temp);
                    counter++;

                }
                string dataStr = @"[[""Hour"",""PowerOutcome""]";
                string druhapulka = "";
                foreach (predictionFormat x in list)
                {
                    druhapulka = druhapulka + $@",[{x.datum},{Math.Round(x.cislo, 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}]";
                }

                druhapulka = druhapulka + "]";

                dataStr += druhapulka;
            
            prepared.Add(dataStr);
            }
            prepared.Add(GraphPreparer.prepareAllDays(data));
            
            return  prepared;  
        }

       

    }
}
