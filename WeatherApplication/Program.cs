using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://api.openweathermap.org/data/2.5/weather?q=Freiberg&appid=81465b514607845ee21f943fc0f53acd&units=metric";
            
            HttpClient web = new HttpClient();
            
            string json = web.GetStringAsync(url).Result;

            CurrentWeather currentweather = JsonConvert.DeserializeObject<CurrentWeather>(json);

        }
    }
}
