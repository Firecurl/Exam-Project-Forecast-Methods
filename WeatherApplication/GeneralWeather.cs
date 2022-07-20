using System.IO;
using System.Collections.Generic;
using System;

public class GeneralWeather
{

    // reading data from csv file 

    private List<WeatherData> list;

    public GeneralWeather()
    {
        list = new List<WeatherData>();
        string[] lines = File.ReadAllLines(@"../Helpful_Data/General_Weather.csv");
        string [][] data = new string[lines.Length][];
        for(int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
            string destination = data[i][0];
            string season = data[i][1];
            int temperature = Int32.Parse(data[i][2]);
            int rain = Int32.Parse(data[i][3]);

            list.Add(new WeatherData {Destination = destination, Season = season, Temperature = temperature, Rain = rain});
        }
    }

    // list in array, only get because should not be able to change something
    public WeatherData[] getAll()
    {
        return list.ToArray();
    }

    // add further destinations
    public void append (string destination, string season, int temperature, int rain)
    {
        list.Add(new WeatherData {Destination = destination, Season = season, Temperature = temperature, Rain = rain});
    }

    // destructor for update the origin file
    ~GeneralWeather()
    {
        string[] data = new string[list.Count];
        for(int i=0; i< list.Count; i++)
        {
            data[i] = list[i].Destination + "," + list[i].Season + "," + list[i].Temperature + "," + list[i].Rain;
        }

        File.WriteAllLines(@"Helpful_Data/General_Weather.csv", data);
    }

    

public void output()
{
    // Output of possible places -> user know which places are availabe for genereal weather
    for (int i=0; i<list.Count; i++)
    {
        Console.WriteLine("Availabe places: ", list[i].Destination);
    }

        // which place?

        Console.WriteLine("For which place do you want to see the general Weather?");
        string place = Console.ReadLine();

        for (int i=0; i<list.Count; i++)
        {
            if(place==list[i].Destination)
            {
                Console.WriteLine("Destination: " + list[i].Destination + "\n" + 
                                "Season: " + list[i].Season + "\n" +
                                "Temperature: " + list[i].Temperature + "°C" + "\n" + 
                                "Raindays: " + list[i].Rain);
            }
        }

    }

} 


