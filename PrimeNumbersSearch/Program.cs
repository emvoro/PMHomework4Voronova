using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;
using System.IO;

namespace PrimeNumbersSearch
{
    class Program
    {
        static bool IsPrime(int x)
        {
            if (x < 2)
                return false;
            for (int i = 2; i <= x / i; i++)
                if (x % i == 0)
                    return false;
            return true;
        }

        static void Main(string[] args)
        {
            bool success = true;
            string error = null;
            List<int> primes = new List<int>();

            Settings settings;

            TimeSpan timeSpan;
            Stopwatch stopwatch = new Stopwatch();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            stopwatch.Start();

            try
            {
                settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"));
                if (settings.PrimesFrom < settings.PrimesTo)
                {
                    for (int i = settings.PrimesFrom; i <= settings.PrimesTo; i++)
                    {
                        if (IsPrime(i))
                            primes.Add(i);
                    }
                }
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is JsonException)
            {
                success = false;
                error = "settings.json are missing or corrupted";
                primes = null;
            }

            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;

            var result = new Result(success, error, timeSpan.ToString(), primes);
            var json = JsonSerializer.Serialize(result, options);
            File.WriteAllText("result.json", json);
        }
    }
}
