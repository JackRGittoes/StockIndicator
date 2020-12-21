using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace StockIndicator
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Currently supported retailers:\nCurrys\nArgos\nAmazon\nGame\nScan\nSmyths\n\n");
            var sleepTime = SleepTimer();
            var urls = GetURL();
            
            await IsInStockAsync(urls, sleepTime);
            Console.WriteLine("No Items left to track, press enter to exit");
        }


        public static async Task<bool> IsInStockAsync(List<string> urls, int sleepTime)
        {
            var result = false;

            while (result == false)
                for (int i = 0; i < urls.Count; i++)
                {
                    var retailer = StockChecker.WhatRetailer(urls[i]);    
                    
                    Console.WriteLine($"\nChecking For {retailer} {i + 1} Stock...");
                    if (retailer == null)
                    {
                        Console.WriteLine($" your {retailer} URL is invalid, Removing from list");
                        urls.Remove(urls[i]);
                    }
                    else
                    {
                        result = await StockChecker.StockCheckerAsync(urls[i], retailer);
                    }

                    if (result == true)
                    {
                        Console.WriteLine($"\n{retailer}'s Item is in stock\nURL: {urls[i]}");
                        urls.Remove(urls[i]);
                        if (urls.Count >= 1)
                        {
                            result = false;
                        }
                    }
                    else if (retailer == null && result == false)
                    {

                    }
                    else if (result == false)
                    {
                        Console.WriteLine($"{retailer}'s item Is Out Of Stock");
                        //Timeout before starting checks again
                        Thread.Sleep(sleepTime);
                    }

                    if(urls.Count == 0)
                    {
                        return true;
                    }
                }
            return true;
        }

        public static int SleepTimer()
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
            if (sleepTime < 5000)
            {
                return 5000;
            }
            return sleepTime;

        }

        public static List<String> GetURL()
        {
            var noOfUrls = 0;
            List<string> urls = new List<string>();
            bool input = false;
            while (input == false)
            {
                try
                {
                    Console.WriteLine("How many URL's do you want to track");
                    noOfUrls = Convert.ToInt32(Console.ReadLine());
                    if (noOfUrls >= 1)
                    {
                        input = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Input");
                }
            }

            for (int i = 0; i < noOfUrls; i++)
            {
                var counter = i + 1;
                Console.WriteLine("\nInput URL " + counter + ": ");
                var url = Console.ReadLine();
                urls.Add(url);
            }
            return urls;
        }
    }
}
