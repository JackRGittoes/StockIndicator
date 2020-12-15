using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace StockIndicator
{
    public class Program
    {
         
        public static async Task Main()
        {
            
           Console.WriteLine("Input URL");
           string url = Console.ReadLine();
            var result = false;
            // Infinite loop
            
            while (true)
            {
                if (url.ToLower().Contains("currys"))
                {
                   result = await Currys.CurrysStockAsync(url);
                }
                
                if (result == false)
                {
                    Console.WriteLine("\n Out of stock");
                }
                else
                {
                    Console.WriteLine("\n In stock");
                }
                Thread.Sleep(5000);
            }

        }

        
    }
}
