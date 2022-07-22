using System.Collections.Generic;
using System;
using System.Text;

public interface IOWM_Weather
{
    void PrintWeather(string city, string units, double time);
}

public static class TimeConverter
{
    public static DateTime UnixTimestampToDateTime(double unixTime)
    {
        DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        long unixTimeStampInTicks = (long) (unixTime * TimeSpan.TicksPerSecond);
        return new DateTime(unixStart.Ticks + unixTimeStampInTicks, DateTimeKind.Utc).ToLocalTime();
    } 

    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
        return (double) unixTimeStampInTicks / TimeSpan.TicksPerSecond;
    }
}

public class CurrentWeather : IOWM_Weather
{
    public List<weather> weather {get; set;}
    public main main {get; set;}
    public double visibility {get; set;}
    public wind wind {get; set;}
    public clouds clouds {get; set;}
    public rain rain {get; set;}
    public sys sys {get; set;}

    public void PrintWeather(string city, string units, double time)
    {
        Console.WriteLine($"Printing Weather for {city}");
        Console.WriteLine( "---------------------{0}\n", new string('-', city.Length));
        Console.WriteLine(TimeConverter.UnixTimestampToDateTime(time));
        
        Console.Write(weather[0]);
        Console.Write(main.ToString(units));
        Console.Write(clouds);
        Console.Write(wind.ToString(units));
        Console.Write($"Visibility: \t{visibility}m\n");
        
        if (rain != null)
            Console.Write(rain);

        Console.WriteLine(sys);
        Console.WriteLine();
        
    }
}

public class UpTo5DaysWeather : IOWM_Weather
{
    public List<list> list {get; set;}

    public void PrintWeather(string city, string units, double time)
    {
        Console.WriteLine($"Printing Forecast for {city}");
        Console.WriteLine( "----------------------{0}\n", new string('-', city.Length));
        foreach (var item in list)
        {
            if ( (item.dt - time) <= 10800 && (time - item.dt) <= 10800)
            {
                Console.WriteLine(TimeConverter.UnixTimestampToDateTime(item.dt));
                Console.Write(item.weather[0]);
                Console.Write(item.main.ToString(units));
                Console.Write(item.clouds);
                Console.Write(item.wind.ToString(units));
                Console.Write("Visibility: \t{0}m\n", item.visibility);
                
                if (item.rain != null)
                    Console.Write(item.rain);

                Console.WriteLine();
            }          
        }
    }
}

public class list
{
    public double dt {get; set;}
    public main main {get; set;}
    public List<weather> weather {get; set;}
    public clouds clouds {get; set;}
    public wind wind {get; set;}
    public double visibility {get; set;}
    public rain rain {get; set;}
}

public class weather
{
    public string main {get; set;}
    public string description {get; set;}

    public override string ToString()
    {
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Weather: \t{main}");          
        output.AppendLine($"Description: \t{description}");
        return output.ToString();
    }
}

public class main
{
    public double temp {get; set;}
    public double feels_like {get; set;}
    public double temp_min {get; set;}
    public double temp_max {get; set;}
    public double humidity {get; set;}

    public string ToString(string units)
    {
        string temp_unit = "";
        switch ( units )
        {
            case "standard":
                temp_unit = "K";
                break;
            case "metric":
                temp_unit = "°C";
                break;
            case "imperial":
                temp_unit = "°F";
                break;
        }
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Temperature: \t{temp}{temp_unit}");
        output.AppendLine($"Feels Like: \t{feels_like}{temp_unit}");
        output.AppendLine($"Temp. min: \t{temp_min}{temp_unit}");
        output.AppendLine($"Temp. max: \t{temp_max}{temp_unit}");
        output.AppendLine($"Humidity: \t{humidity}%");

        return output.ToString();
    }
    
}

public class wind
{
    public double speed {get; set;}
    public double deg {get; set;}
    public double gust {get; set;}

    public string ToString(string units)
    {
        string speed_unit = "";
        switch ( units )
        {
            case "standard":
            case "metric":
                speed_unit = "m/s";
                break;
            case "imperial":
                speed_unit = "mi/h";
                break;
        }
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Wind speed: \t{speed}{speed_unit}");
        output.AppendLine($"Wind degree: \t{deg}°");
        output.AppendLine($"Gust speed: \t{gust}{speed_unit}");

        return output.ToString();
    }
}

public class rain
{
    public double? _1h {get; set;}
    public double? _3h {get; set;}

    public override string ToString()
    {
        StringBuilder output = new StringBuilder();
        if ( _1h != null )
        {
            output.AppendLine($"Rain last 1h: \t{_1h}mm");
        }
        if ( _3h != null )
        {
            output.AppendLine($"Rain last 3h: \t{_3h}mm");
        }
        return output.ToString();
    }
}

public class clouds
{
    public int all {get; set;}

    public override string ToString()
    {
        return $"Cloudiness: \t{all}%\n";
    }
}

public class sys
{
    public double sunrise {get; set;}
    public double sunset {get; set;}

    public override string ToString()
    {
        StringBuilder output = new StringBuilder();
        output.Append("Sunrise: \t");
        output.AppendLine(TimeConverter.UnixTimestampToDateTime(sunrise).ToString());
        output.Append("Sunset: \t");
        output.AppendLine(TimeConverter.UnixTimestampToDateTime(sunset).ToString());

        return output.ToString();
    }
}