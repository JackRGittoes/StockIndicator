using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockIndicator
{
    public class Argos
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HtmlDocument htmlDoc = new HtmlDocument();
        public static async Task<bool> ArgosStockAsync(string url)
        {
            bool IsTrue = false;
            while (IsTrue == false)
            {
                var responseBody = await client.GetStringAsync(url);
                htmlDoc.LoadHtml(responseBody);

                try
                {
                    foreach (var item in htmlDoc.DocumentNode.SelectNodes("//strong"))
                    {
                       
                        if (item.InnerText.Contains("Not available"))
                        {
                           return false;
                        }
                        
                    };
                }
                catch (NullReferenceException)
                {
                    return true;
                }
                IsTrue = true;
            }
            return true;
        }
    }
}
