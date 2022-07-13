using System.Collections.Generic;

public class UpTo5DaysWeather
{
    public List<weather> weather {get; set;}
    public main main {get; set;}
    public double visibility {get; set;}
    public wind wind {get; set;}
    public rain rain {get; set;}
    public clouds clouds {get; set;}
    public sys sys {get; set;}
}

public class weather
{
    public string main {get; set;}
    public string description {get; set;}
}

public class main
{
    public double temp {get; set;}
    public double feels_like {get; set;}
    public double temp_min {get; set;}
    public double temp_max {get; set;}
    public double humidity {get; set;}
    
}

public class wind
{
    public double speed {get; set;}
    public int deg {get; set;}
    public double gust {get; set;}
}

public class rain
{
    public double _1h {get; set;}
    public double _3h {get; set;}
}

public class clouds
{
    public int all {get; set;}
}

public class sys
{
    public double sunrise {get; set;}
    public double sunset {get; set;}
}