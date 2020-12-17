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
        public const string amazonNodeContains = "Add to Basket";
        public const string argosNodeContains= "Not available";
        public const string currysNodeContains = "Add to basket";


        public static async Task<bool> StockCheckerAsync(string url, int retailer)
        {
            var node = " ";
            var nodeContains = " ";
            switch(retailer)
            {
                case 1:
                    node = currysNode;
                    nodeContains = currysNodeContains;
                    break;
                case 2:
                    node = argosNode;
                    nodeContains = argosNodeContains;
                    break;
                case 3: 
                    node = amazonNode;
                    nodeContains = amazonNodeContains;
                    break;
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

                        if (item.InnerText.Contains(nodeContains) && retailer == 2)
                        {
                            return false;
                        }
                        else if(item.InnerText.Contains(nodeContains))
                        {
                            return true;
                        }

                    };
                }
                catch (NullReferenceException)
                {
                    if(retailer == 2)
                    {
                        return true;
                    }
                    return false;
                }
                IsTrue = true;
            }
            if(retailer == 2)
            {
                return true;
            }
            return false;
        }
    }
}


