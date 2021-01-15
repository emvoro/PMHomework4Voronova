using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CurrencyConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var uri = new Uri("https://bank.gov.ua");
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri;
            string body = "";
            HashSet<Currency> currencies = new HashSet<Currency>();

            Console.WriteLine(" Currency Converter \n Emilia Voronova\n");

            try
            {
                var response = await httpClient.GetAsync("/NBUStatService/v1/statdirectory/exchange?json");
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
                File.WriteAllText("cache.json", body);
                currencies = JsonConvert.DeserializeObject<HashSet<Currency>>(File.ReadAllText("cache.json"));
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                {
                    Console.WriteLine(" The file is not found.");
                    return;
                }
                else
                    Console.WriteLine($" There was an error while updating currencies.\n You can continue with the last updated data..");
            }
            
            Console.WriteLine(" Enter initial currency :");
            string initialCurrency = Console.ReadLine().Trim().ToUpper();
            while (initialCurrency.Trim().Length != 3)
            {
                Console.WriteLine(" Enter initial currency :");
                initialCurrency = Console.ReadLine().Trim().ToUpper();
            }
            Console.WriteLine(" Enter desired currency :");
            string desiredCurrency = Console.ReadLine().Trim().ToUpper();
            while (desiredCurrency.Trim().Length != 3)
            {
                Console.WriteLine(" Enter desired currency :");
                desiredCurrency = Console.ReadLine().Trim().ToUpper();
            }
            Console.WriteLine(" Enter amount :");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine().Replace(',', '.'), NumberStyles.Any, nfi, out amount) || amount <= 0)
                Console.WriteLine(" Enter amount :");

            decimal currency = 0;
            decimal result = 0;
            bool isValid = false;
            if (currencies.Any(x => x.Cc == initialCurrency) && currencies.Any(x => x.Cc == desiredCurrency))
            {
                decimal initialCourse = currencies.Where(x => x.Cc == initialCurrency).Select(x => x.Rate).First();
                decimal desiredCourse = currencies.Where(x => x.Cc == desiredCurrency).Select(x => x.Rate).First();
                currency = initialCourse / desiredCourse;
                result = amount * currency;
                isValid = true;
            }
            else if ((currencies.Any(x => x.Cc == initialCurrency) || currencies.Any(x => x.Cc == desiredCurrency))
                && (initialCurrency == "UAH" || desiredCurrency == "UAH"))
            {
                if (initialCurrency == "UAH")
                {
                    decimal desiredCourse = currencies.Where(x => x.Cc == desiredCurrency).Select(x => x.Rate).First();
                    currency = desiredCourse;
                    result = amount / currency;
                }
                else if (desiredCurrency == "UAH")
                {
                    decimal initialCourse = currencies.Where(x => x.Cc == initialCurrency).Select(x => x.Rate).First();
                    currency = initialCourse;
                    result = amount * currency;
                }
                isValid = true;
            }

            if (isValid || initialCurrency == desiredCurrency)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n Initial amount : {amount} {initialCurrency}" +
                                  $"\n Currency       : {Math.Round(currency, 2)} " +
                                  $"\n Converted      : {Math.Round(result, 2)} {desiredCurrency} " +
                                  $"\n From           : {DateTime.Now.ToShortDateString()}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine($"\n Error. {initialCurrency}, {desiredCurrency} pair is not found."); 
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
