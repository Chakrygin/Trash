using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var tzs = TimeZoneInfo.GetSystemTimeZones();

            foreach (var tz in tzs)
            {
                Console.WriteLine($"{tz.DisplayName} - {tz.StandardName} - {tz.BaseUtcOffset}");
            }
        }
    }
}