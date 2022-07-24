using System;

public class WeatherData
{
    public string Destination {get; set;}
    public string Season {get; set;}
    public int Temperature {get; set;}
    public int Rain {get; set;}

    public WeatherData(string destination, string season, int temperature, int rain)
    {
        Destination = destination;
        Season = season;
        Temperature = temperature;
        Rain = rain;
    }

    public override string ToString()
    {
        return Destination + "|" + Season + "|" + Temperature + "|" + Rain; 
    }
}