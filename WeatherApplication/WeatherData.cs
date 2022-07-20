using System;

public class WeatherData
{
    public string Destination {get; set;}
    public string Season {get; set;}
    public int Temperature {get; set;}
    public int Rain {get; set;}

    public override string ToString()
    {
        return Destination + "|" + Season + "|" + Temperature + "|" + Rain; 
    }
}