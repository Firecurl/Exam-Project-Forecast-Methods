using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

public enum TypeOfWeather {General=1, Current=2, Forecast=3}

namespace WeatherApplication
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("\n\n\n\n");                   
            TypeOfWeather type = AskForTypeOfWeather();
            Console.Clear();
            
            switch (type)
            {
                case TypeOfWeather.General:
                    //create Instance of GeneralWeather and get Data
                    GeneralWeather weather = new GeneralWeather();
                    weather.output();
                    break;
                case TypeOfWeather.Current:
                case TypeOfWeather.Forecast:
                    //create Instance of Up25DaysWeather and get Data
                    OMW_WeatherRequest request = new OMW_WeatherRequest("81465b514607845ee21f943fc0f53acd", type);
                    request.SetParameters();
                    request.BuildUrlString();                    
                    var weatherInfo = request.RequestWeather();
                    Console.Clear();
                    weatherInfo.PrintWeather(request.city, request.units);
                    break;
            }               
        }

        static TypeOfWeather AskForTypeOfWeather()
        {
            string input;            
            
            while (true)
            {
                Console.WriteLine("Type of weather information:");
                Console.WriteLine("  1: General weather for a country");
                Console.WriteLine("  2: Current Weather for a specific City");
                Console.WriteLine("  3: Forecast for a specific City");
                Console.WriteLine();
                Console.Write("Answer: ");
                input = Console.ReadLine();
                Console.WriteLine();

                if ( input.Equals("1") || input.Equals("2") || input.Equals("3") )
                {
                    return (TypeOfWeather) Int32.Parse(input);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Answer!");
                Console.WriteLine("Try again.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
    }

    class OMW_WeatherRequest
    {
        public TypeOfWeather typeOfWeather {get;} 
        private readonly string appid;       
        public string city {get; set;}
        public string units {get; set;}
        public string url {get; set;}

        public OMW_WeatherRequest(string appid, TypeOfWeather type)
        {
            this.appid = appid;
            this.typeOfWeather = type;
        }

        public IOWM_Weather RequestWeather()
        {
            HttpClient web = new HttpClient();
            string json = web.GetStringAsync(this.url).Result;
            TestForRain(ref json);

            if ( typeOfWeather == TypeOfWeather.Current )
            {
                CurrentWeather weather = JsonConvert.DeserializeObject<CurrentWeather>(json);
                return weather;
            }
            else
            {
                UpTo5DaysWeather weather = JsonConvert.DeserializeObject<UpTo5DaysWeather>(json);
                return weather;
            }
        }

        private void TestForRain(ref string json)
        {
            int index;
            
            while ( json.IndexOf("\"1h\"") > 0 || json.IndexOf("\"3h\"") > 0 )
            {
                if ( (index = json.IndexOf("\"1h\"")) > 0 )
                    json = json.Insert(index+1, "_");
                if ( (index = json.IndexOf("\"3h\"")) > 0 )
                    json = json.Insert(index+1, "_");
            }
        }

        public void BuildUrlString()
        {
            string url = "https://api.openweathermap.org/data/2.5/";

            switch (this.typeOfWeather)
            {
                case TypeOfWeather.Current:
                    url += "weather?";
                    break;
                case TypeOfWeather.Forecast:
                    url += "forecast?";
                    break;
            }

            url += "q=" + city + "&";
            url += "appid=" + appid + "&";
            url += "units=" + units;

            this.url = url;
        }

        public void SetParameters()
        {
            string input;
            
            Console.WriteLine("Type in specific city:");
            Console.WriteLine();
            Console.Write("Answer: ");
            input = Console.ReadLine();
            this.city = input;
            
            Console.Clear();
            
            Console.WriteLine("Units:");
            Console.WriteLine("  standard: e.g. Temperature in \"Kelvin\"");
            Console.WriteLine("  metric:   e.g. Temperature in \"Clesius\"");
            Console.WriteLine("  imperial: e.g. Temperature in \"Fahrenheit\"");
            Console.WriteLine();
            Console.Write("Answer: ");
            input = Console.ReadLine();

            if (input != "")
            {
                this.units = input;
            }
            else
            {
                this.units = "standard";
            }            
            
            Console.Clear();
        }
    }
}
