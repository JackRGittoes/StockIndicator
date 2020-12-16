using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockIndicator
{
    class Amazon
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HtmlDocument htmlDoc = new HtmlDocument();
        public static async Task<bool> AmazonStockAsync(string url)
        {
            bool IsTrue = false;
            while (IsTrue == false)
            {
                var responseBody = await client.GetStringAsync(url);
                htmlDoc.LoadHtml(responseBody);

                try
                {
                    foreach (var item in htmlDoc.DocumentNode.SelectNodes("//div"))
                    {

                        if (item.InnerText.Contains("Add to Basket"))
                        {
                            return true;
                        }

                    };
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                IsTrue = true;
            }
            return false;
        }
    }
}


