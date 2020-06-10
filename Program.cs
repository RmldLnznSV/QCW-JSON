using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetJsonSample
{
    class Program
    {
        public class PersonalInfo
        {
            public string City { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }

            [JsonExtensionData]
            public Dictionary<string, object> ExtensionData { get; set; }

        }

        static void Main()
        {
            //Demo1();
            
            //Source: https://www.ncdc.noaa.gov/cag/global/time-series/southAmerica/land/1/6/1900-2020
            string json = File.ReadAllText(@"..\..\..\json\temperatures-years.json");

            using (JsonDocument document = JsonDocument.Parse(json))
            {
                double sumOfAllTemperatures = 0;
                int count = 0;

                Console.WriteLine("Year | Temperature");

                foreach (JsonProperty property in document.RootElement.GetProperty("data").EnumerateObject())
                {
                    if (double.TryParse(property.Value.GetString(), out double temp))
                    {
                        Console.WriteLine($"{property.Name} | {temp}");

                        sumOfAllTemperatures += temp;
                        count++;
                    }
                }

                double averageTemp = sumOfAllTemperatures / count;
                Console.WriteLine($"Average Temp:  {averageTemp}");
            }
        }

        private static void Demo1()
        {
            string json = File.ReadAllText(@"..\..\..\json\personalInfo.json");

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            var personalInfo = JsonSerializer.Deserialize<PersonalInfo>(json, options);

            Console.WriteLine(personalInfo.Name);
            Console.WriteLine(personalInfo.ExtensionData.Count);
        }
    }
}
