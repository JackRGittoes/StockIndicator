using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockIndicator
{
    class StockChecker
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HtmlDocument htmlDoc = new HtmlDocument();

        //Values to check stock on each retailer
        public const string amazonNode = "//div";
        public const string argosNode = "//strong";
        public const string currysNode = "//div";
        public const string gameNode = "//div";
        public const string scanNode = "//div";
        public const string smythsNode = "//div";

        public const string amazonNodeContains = "Add to Basket";
        public const string argosNodeContains = "Not available";
        public const string currysNodeContains = "Add to basket";
        public const string gameNodeContains = "out of stock";
        public const string scanNodeContains = "In stock";
        public const string smythsNodeContains = "Out of Stock";


        public static string WhatRetailer(string url)
        {
            var retailer = " ";
            if (url.ToLower().Contains("currys"))
            {
                retailer = "currys";
            }
            else if (url.ToLower().Contains("argos"))
            {
                retailer = "argos";
            }
            else if (url.ToLower().Contains("amazon"))
            {
                retailer = "amazon";

            }
            else if(url.ToLower().Contains("game.co.uk"))
            {
                retailer = "game";
            }
            else if(url.ToLower().Contains("scan"))
            {
                retailer = "scan";
            }
            else if(url.ToLower().Contains("smyths"))
            {
                retailer = "smyths";
            }
            return retailer;
        }

        public static async Task<bool> StockCheckerAsync(string url, string retailer)
        {
            var node = " ";
            var nodeContains = " ";
            if (retailer.Contains("currys"))
            {
                node = currysNode;
                nodeContains = currysNodeContains;
            }
            else if (retailer.Contains("argos"))
            {
                node = argosNode;
                nodeContains = argosNodeContains;
            }
            else if (retailer.Contains("amazon"))
            {
                node = amazonNode;
                nodeContains = amazonNodeContains;
            }
            else if (retailer.Contains("game"))
            {
                node = gameNode;
                nodeContains = gameNodeContains;
            }
            else if (retailer.Contains("scan"))
            {
                node = scanNode;
                nodeContains = scanNodeContains;
            }
            else if(retailer.Contains("smyths"))
            {
                node = smythsNode;
                nodeContains = smythsNodeContains;
            }


            bool IsTrue = false;
            while (IsTrue == false)
            {
                var responseBody = await client.GetStringAsync(url);
                htmlDoc.LoadHtml(responseBody);

                try
                {
                    foreach (var item in htmlDoc.DocumentNode.SelectNodes(node))
                    {

                        //If the retailer is argos returns false because we are tracking an out of stock text element
                        if (item.InnerText.Contains(nodeContains) && retailer.Contains("argos") || item.InnerText.Contains(nodeContains) && retailer.Contains("game") || item.InnerText.Contains(nodeContains) && retailer.Contains("smyths"))
                        {
                            return false;
                        }
                        else if (item.InnerText.Contains(nodeContains))
                        {
                            return true;
                        }

                    };
                }
                catch (NullReferenceException)
                {
                    if (retailer.Contains("argos") || retailer.Contains("game") || retailer.Contains("smyths"))
                    {
                        return true;
                    }
                    return false;
                }
                IsTrue = true;
            }
            if (retailer.Contains("argos") || retailer.Contains("game") || retailer.Contains("smyths"))
            {
                return true;
            }
            return false;
        }
    }
}