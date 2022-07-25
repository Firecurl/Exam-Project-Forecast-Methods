using System;
using System.Collections.Generic;

class Program_Test
{
    
    static void Main(string[] args)
    {
        TestReadingCSV();
    }

    static void TestReadingCSV()
    {
        GeneralWeather test = new GeneralWeather();
        var array = test.getAll();

        string[] states = new string[] {"Australia", "US", "Brasil", "Spain", "Germany", "Great Britain", 
                                        "Italy", "Vietnam", "Japan", "Turkiye", "Egypt", "South Africa"};

        for (int i = 0; i < array.Length; i+=4)
        {
            if ( array[i].Destination == states[i/4])
            {
                Console.WriteLine("Successfully readed {0}. state ({1})", i/4, states[i/4]);
            }
            else
            {
                Console.WriteLine("Not successfully readed {0}. state ({1})", i/4, states[i/4]);
            }
        }
    }
}