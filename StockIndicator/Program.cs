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
            var sleepTime = CheckStockTimer();
            var url = GetURL();      
            var retailer = WhatRetailer(url);
            var stock = await Checker(retailer, url);
            await IsInStockAsync(stock, url, retailer, sleepTime);
           
        }

        public static int CheckStockTimer()
        {
            int sleepTime;
            try
            {
                Console.WriteLine("Input in Ms how often you want to check for stock (e.g. 5000 = 5 seconds)\nMinimum 5 seconds");
                sleepTime = Convert.ToInt32(Console.ReadLine());

            }
            catch (FormatException)
            {
                Console.WriteLine("Defaulted to 5 seconds");
                return 5000;
            }
            if(sleepTime < 5000)
            {
                return 5000;
            }
            return sleepTime;
            
        }
        public static string GetURL()
        {
            Console.WriteLine("\nInput URL");
            var url = Console.ReadLine();
            return url;
        }

        public static int WhatRetailer(string url)
        {
            /* 1 = Currys
             * 2 = Argos
             * 3 = Amazon 
             */

            //Returns an integer value representing which retailer
            if (url.ToLower().Contains("currys"))
            {
                return 1;
            }
            else if (url.ToLower().Contains("argos"))
            {
                return 2;
            }
            else if (url.ToLower().Contains("amazon"))
            {
                return 3;
            }
            return 0;
        }
        
        public static async Task<bool> Checker(int retailer, string url)
        {
            var isTrue = false;
            var result = false;
            while (isTrue == false)
            {
                if (retailer == 0)
                {
                    Console.WriteLine("Invalid URL");
                    url = GetURL();
                    retailer = WhatRetailer(url);
                    
                }
                else
                {
                    result = await StockChecker.StockCheckerAsync(url, retailer);
                    break;
                }                              
            }
            return result;
        }

        public static async Task<bool> IsInStockAsync(bool result, string url, int retailer, int sleepTime)
        {
            while (result == false)
            {

                Console.WriteLine("Out Of Stock");

                //Timeout before starting checks again
                Thread.Sleep(sleepTime);

                Console.WriteLine("Checking For Stock...");
                await StockChecker.StockCheckerAsync(url, retailer);
            }  
            if(result == true)
            {
                Console.WriteLine("\nItem is in stock\n\nPress Any Key To Exit");
                Console.ReadLine();
            }
            return true;
        }
    }
}
