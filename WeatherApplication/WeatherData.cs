using System;

namespace WeatherApplication
{
    class WeatherData
    {
        public string destination {get; set;}
        public string season {get; set;}
        public int temperature {get; set;}
        public int rain {get; set;}

        public override string To.String()
        {
            return Destination + "|" + Season + "|" + Temperature + "|" + Raindays; 
        }
    }
}