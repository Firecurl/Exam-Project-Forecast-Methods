using System.IO;
using System.Collections.Generic;
using System;

namespace WeatherApplication
{
    class GeneralWeather
    {

        // reading data from csv file 

        private List<WeatherData> liste;

        public Table()
        {
            liste = new List<WeatherData>();
            string[] zeilen = File.ReadAllLines(@"General_Weather.csv");
            foreach(string zeile in zeilen)
            {
                string[]data = zeile.Split(';');
                string destination = data[0];
                string season = data[1];
                int temperature = data[2];
                int rain = data[3];

                liste.Add(new WeatherData {Destination = destination, Season = season, Temperature = temperature, Raindays = rain});
            }

        }
        static void Main(string[] args)
        {

        }
    }
} 