using System.IO;
using System.Collections.Generic;
using System;

namespace WeatherApplication
{
    class GeneralWeather
    {

        // reading data from csv file 

        private List<WeatherData> list;

        public Table()
        {
            list = new List<WeatherData>();
            string[] zeilen = File.ReadAllLines(@"General_Weather.csv");
            foreach(string zeile in zeilen)
            {
                string[]data = zeile.Split(';');
                string destination = data[0];
                string season = data[1];
                int temperature = data[2];
                int rain = data[3];

                list.Add(new WeatherData {Destination = destination, Season = season, Temperature = temperature, Raindays = rain});
            }

            // list in array, only get because should not be able to change something
            public WeatherData [] getAll()
            {
                return list.ToArray();
            }

            // add further destinations
            public void append (string destination, string season, int temperature, int rain)
            {
                list.Add(new WeatherData {Destination = destination, Season = season, Temperature = temperature, Raindays = rain});
            }

            // destructor for update the origin file

            ~GeneralWeather()
            {
                string[] data = new string[list.Count];
                for(int i=0; i< list.Count; i++)
                {
                    data[i] = list[i].Destination + ";" + list[i].Season ";" + list[i].Temperature ";" + list[i].Raindays;
                }

                File.WriteAllLines(@"General_Weather.csv", data);
            }

        }
        static void Main(string[] args)
        {

        }
    }
} 