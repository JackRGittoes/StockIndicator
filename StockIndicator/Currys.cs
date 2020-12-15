using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockIndicator
{
    public class Currys
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HtmlDocument htmlDoc = new HtmlDocument();
        public static async Task<bool> CurrysStockAsync(string url)
        {

            for (int i = 0; i < 100; i++)
            {

                var responseBody = await client.GetStringAsync(url);
                htmlDoc.LoadHtml(responseBody);

                try
                {


                    foreach (var item in htmlDoc.DocumentNode.SelectNodes("//*[@data-button-label]"))
                    {
                        if (item.GetAttributeValue("data-button-label", "").Contains("Add to basket"))
                        {
                            return true;    
                        }
                        return false;
                    };
                }
                catch (NullReferenceException e)
                {
                    return false;
                }
            }
            return false;

        }
    }
}
