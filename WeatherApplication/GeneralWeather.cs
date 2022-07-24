using System.IO;
using System.Collections.Generic;
using System;

/// <include file='DocuGeneralWeather.xml' path='DocuGeneralWeather/members[@name="GeneralWeather"]/*'/>

public class GeneralWeather
{

    // reading data from csv file 

    private List<WeatherData> list = new List<WeatherData>();

    public GeneralWeather()
    {        
        string[] lines = File.ReadAllLines(@"../Helpful_Data/General_Weather.csv");
        string [][] data = new string[lines.Length][];
        for(int i = 1; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
            
            string destination = data[i][0].Trim();
            string season = data[i][1].Trim();
            int temperature = Int32.Parse(data[i][2].Trim());
            int rain = Int32.Parse(data[i][3].Trim());
            
            list.Add(new WeatherData(destination, season, temperature, rain));
        }
    }
    
    
    /// <include file='DocuGeneralWeather.xml' path='DocuGeneralWeather/members[@name="getAll"]/*'/>

    // list in array, only get because should not be able to change something
    public WeatherData[] getAll()
    {
        return list.ToArray();
    }
    
    
    /// <include file='DocuGeneralWeather.xml' path='DocuGeneralWeather/members[@name="append"]/*'/>

    // add further destinations
    public void append (string destination, string season, int temperature, int rain)
    {
        list.Add(new WeatherData(destination, season, temperature, rain));
    }
    
    
    /// <include file='DocuGeneralWeather.xml' path='DocuGeneralWeather/members[@name="~GeneralWeather"]/*'/>

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

    
    /// <include file='DocuGeneralWeather.xml' path='DocuGeneralWeather/members[@name="Output"]/*'/>

    public void output()
    {
        // Output of possible places -> user know which places are availabe for genereal weather
        Console.WriteLine("Availabe places:");
        for (int i = 0; i < list.Count; i+=4)
        {
            Console.WriteLine("\t- " + list[i].Destination);
        }
        Console.WriteLine();

        // which place?
        string place;
        int index;

        do
        {
            Console.WriteLine("For which place do you want to see the general Weather?");
            place = Console.ReadLine();
            
            index = list.FindIndex(x => x.Destination.ToLower() == place.ToLower());
            if ( index != -1 )
                break;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong Answer!");
            Console.WriteLine("Try again.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

        } while ( true );
        
        // Output of weather for chosen place
        Console.Clear();
        Console.WriteLine("General Weather for {0}", list[index].Destination);
        Console.WriteLine("--------------------{0}\n", new string('-', place.Length));       
        
        for ( int i = 0; i < 4; i++ )
        {
            Console.WriteLine( // "Destination:\t" + list[i].Destination + "\n" + 
                        "Season:\t\t" + list[index+i].Season + "\n" +
                        "Temperature:\t" + list[index+i].Temperature + "°C" + "\n" + 
                        "Raindays:\t" + list[index+i].Rain);
            Console.WriteLine();
        }
    }

    private bool IsAvailablePlace(string place, out int index)
    {
        index = 0;
        for (int i = 0; i < list.Count; i+=4)
            if ( list[i].Destination.ToLower() == place.ToLower() )
                return true;
            else
                index += 4;

        return false;
    }

}
