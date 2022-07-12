using System;
using System.Net.Http;
using Newtonsoft.Json;

public enum TypeOfRequest {General, Current, Forecast}

namespace WeatherApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            UrlStringBuilder urlBuilder = new UrlStringBuilder();
            
            string url = "https://api.openweathermap.org/data/2.5/weather?q=Nagpur&appid=81465b514607845ee21f943fc0f53acd&units=metric";
            
            HttpClient web = new HttpClient();
            
            string json = web.GetStringAsync(url).Result;

            urlBuilder.TestForRain(ref json);

            CurrentWeather currentweather = JsonConvert.DeserializeObject<CurrentWeather>(json);
            Console.WriteLine(currentweather.rain._1h);
            Console.WriteLine(json);

            //Console.WriteLine(currentweather.rain._1h);
            

        }
    }

    class UrlStringBuilder
    {
        public int typeOfRequest {get; set;}        
        public string city {get; set;}
        public string units {get; set;}

        public void TestForRain(ref string json)
        {
            int index;
            
            while ( json.IndexOf("\"1h\"") > 0 || json.IndexOf("\"3h\"") > 0 )
            {
                Console.WriteLine("Rain oder Snow erkannt");
                if ( (index = json.IndexOf("\"1h\"")) > 0 )
                    json = json.Insert(index+1, "_");
                if ( (index = json.IndexOf("\"3h\"")) > 0 )
                    json = json.Insert(index+1, "_");
            }
        }

        public void AskForParameters()
        {
            Console.WriteLine("Type of weather information:");
            Console.WriteLine("  1: General weather for a country");
            Console.WriteLine("  2: Current Weather for a specific City");
            Console.WriteLine("  3: Forecast for a specific City");
            Console.WriteLine();
            Console.WriteLine("Answer: ");
            string input;
            
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    this.typeOfRequest = (int)TypeOfRequest.General;
                    break;
                case "2":
                    this.typeOfRequest = (int)TypeOfRequest.General;
                    break;
                case "3":
                    this.typeOfRequest = (int)TypeOfRequest.General;
                    break;
                default:
                    Console.WriteLine("Invaild Answer");
                    break;

            };
        }

    }
}
