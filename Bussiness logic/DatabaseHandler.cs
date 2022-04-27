using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace BakalarskaPrace1.Bussiness_logic
{
    public class DatabaseHandler
    {
        private string connectionst = @"URI=file:database.db";
        public DatabaseHandler()
        {
            //Console.WriteLine(connectionst);
        }
        private void deleteDays()
        {
            string stm = "delete FROM day where id_forecast>0";
            using var con = new SQLiteConnection(connectionst);
            con.Open();
            using var cmd = new SQLiteCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void Savetodb(List<predictionFormat> data)
        {
            deleteDays();
            using var con = new SQLiteConnection(connectionst);
            con.Open();
            foreach (predictionFormat x in data)
            {
                using var cmd = new SQLiteCommand(con);
                cmd.CommandText = "INSERT INTO day(date,powerOutput,id_forecast) VALUES(@date, @power,1)";

                cmd.Parameters.AddWithValue("@date", x.datum);
                cmd.Parameters.AddWithValue("@power",x.cislo);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
        }
        public List<predictionFormat> getData7dbd()
        {
            string cs = @"URI=file:C:\Users\Jano\Documents\test.db";

            using var con = new SQLiteConnection(connectionst);
            con.Open();

            string stm = "SELECT * FROM forecast7dbd";

            var cmd = new SQLiteCommand(stm, con);
             SQLiteDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
           
            if (!rdr.HasRows)
            {
                SolarForecaster sf = new SolarForecaster();
                sf.HledanaElektrarna = sf.testovaciElektr;
                sf.createPrediction(predictionEnum.SDYS);
                List<predictionFormat> news = sf.get7dysPrediction();
                Savetodb(news);

                return news;
            }
            
            else if (DateTime.Compare(DateTime.ParseExact(rdr.GetString(1), "dd.MM.yyyy", null), DateTime.Now)>0)
            {
                SolarForecaster sf = new SolarForecaster();
                sf.HledanaElektrarna = sf.testovaciElektr;
                sf.createPrediction(predictionEnum.SDYS);
                List<predictionFormat> news = sf.get7dysPrediction();
                Savetodb(news);

                return news;
            }

            stm = "SELECT * FROM day";

            cmd = new SQLiteCommand(stm, con);
            rdr = cmd.ExecuteReader();
            List<predictionFormat> datas = new List<predictionFormat>();
            while (rdr.Read())
            {
                predictionFormat temp;
                
                temp.cislo = rdr.GetDouble(1);
                temp.datum = rdr.GetString(0);
                datas.Add(temp);

            }
            return datas;

            
        }
    }
}
