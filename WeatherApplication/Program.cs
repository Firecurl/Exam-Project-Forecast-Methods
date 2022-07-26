using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

/// <include file='DocuProgram.xml' path='DocuProgram/members[@name="Hole File"]/*'/>

public enum TypeOfWeather {General=1, Current=2, Forecast=3}

namespace WeatherApplication
{
    class Program
    {
        /// <include file='DocuProgram.xml' path='DocuProgram/members[@name="Main"]/*'/>
        static void Main(string[] args)
        {      
            Console.Clear();    
            
            /// <include file='DocuProgram.xml' path='DocuProgram/members[@name="TypeWeather"]/*'/>

            TypeOfWeather type = AskForTypeOfWeather();
            
            if ( type == TypeOfWeather.General)
            {
                //create Instance of GeneralWeather and get Data
                GeneralWeather weather = new GeneralWeather();
                Console.Clear();
                weather.output();
            }
            else
            {
                //create Instance of Up25DaysWeather and get Data
                OMW_WeatherRequest request = new OMW_WeatherRequest("81465b514607845ee21f943fc0f53acd", type);
                IOWM_Weather weatherInfo = null;
                
                // Loop if city and units are wrong
                // -> can't get a request from OWM API
                do
                {
                    request.SetParameters();
                    request.BuildUrlString();   

                    
                    string errorMessage = "";
                    try
                    {
                        weatherInfo = request.RequestWeather();
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }                
                    
                    if ( errorMessage == "")
                    {
                        break;
                    }

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("City is not correct!");
                    Console.WriteLine("Try again.");
                    Console.ResetColor();
                    Console.WriteLine();
                } while ( true );

                // If request is current, then just current time is neede
                double time = 0;
                if ( type == TypeOfWeather.Current )
                {
                    time = TimeConverter.DateTimeToUnixTimestamp(DateTime.Now);
                }

                // If request is forecast, then a specific Time and Day is needed
                else
                {
                    UpTo5DaysWeather upTo5DaysWeather = (UpTo5DaysWeather) weatherInfo;
                    do
                    {
                        string errorMessage = "";
                        Console.WriteLine();
                        Console.WriteLine("Which day and which time?");
                        Console.WriteLine("Please type in corret format!\n");
                        Console.WriteLine("DD MM YYYY HH:MM");
                        string input = Console.ReadLine();
                        
                        try
                        {
                            time = TimeConverter.DateTimeToUnixTimestamp(Convert.ToDateTime(input));
                        }
                        catch (FormatException e)
                        {
                            errorMessage = e.Message;
                        }

                        if ( (errorMessage == "") && IsValidTime(time, upTo5DaysWeather) )
                        {
                            break;   
                        }

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Time is not in the correct Range or in a wrong format!");
                        Console.WriteLine("You can only get Weather from {0} to {1}", 
                                        TimeConverter.UnixTimestampToDateTime(upTo5DaysWeather.list[0].dt), 
                                        TimeConverter.UnixTimestampToDateTime(upTo5DaysWeather.list[upTo5DaysWeather.list.Count-1].dt));
                        Console.ResetColor();
                        Console.WriteLine();
                    } while ( true );
                }
                Console.Clear();
                Console.WriteLine("\n");
                weatherInfo.PrintWeather(request.units, time);
            }
            Console.WriteLine("Press any Key to exit");
            Console.ReadKey(); 
        }

        static bool IsValidTime(double time, UpTo5DaysWeather weather)
        {
            double now = TimeConverter.DateTimeToUnixTimestamp(DateTime.Now.ToLocalTime());

            if (time < now)
            {
                Console.WriteLine("too old");
                return false;
            }
            if ( time > (weather.list[weather.list.Count -1]).dt)
            {
                Console.WriteLine("too future");
                Console.WriteLine(TimeConverter.UnixTimestampToDateTime(weather.list[weather.list.Count -1].dt));
                return false;
            }
            return true;
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
    
    /// <include file='DocuProgram.xml' path='DocuProgram/members[@name="OVM"]/*'/>

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
            this.units = "standard";
        }

        public IOWM_Weather RequestWeather()
        {
            HttpClient web = new HttpClient();
            string json = "";

            try
            {
                json = web.GetStringAsync(this.url).Result;
            }
            catch ( HttpRequestException e )
            {
                throw new Exception(e.Message);
            }
            catch ( AggregateException a )
            {
                throw new Exception(a.Message);
            }

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
            
            Console.WriteLine("");
            
            Console.WriteLine("Units:");
            Console.WriteLine("  standard: e.g. Temperature in \"Kelvin\"");
            Console.WriteLine("  metric:   e.g. Temperature in \"Clesius\"");
            Console.WriteLine("  imperial: e.g. Temperature in \"Fahrenheit\"");
            Console.WriteLine();
            Console.Write("Answer: ");
            input = Console.ReadLine();

            if (input == "metric")
            {
                this.units = input;
            }
            if (input == "imperial")
            {
                this.units = input;
            }    
        }
    }
}
