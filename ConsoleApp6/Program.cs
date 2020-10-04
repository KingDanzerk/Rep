using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Alpaca.Markets;
using Nancy.Extensions;
using Nancy.Json;
using Newtonsoft.Json;
using Npgsql;


namespace ConsoleApp6
{
    class Program
    {
        private const string API_KEY = "PKBZ8COU1TZ8SUSCFWKF";

        private const string API_SECRET = "YiQteBuGzBml2V9fb1uerpPMYDbrB7IopTCnM8FQ";
        
        public static async Task Main(string[] args)
        {
            // First, open the API connection
            var client = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET));
            //Console.WriteLine(Filter(1, "QQQ"));
            var assets = GetAssets().ToList().Where(asset => asset.Ticker == "SPY");

            foreach (var item in assets)
            {
                Console.WriteLine(item.SecurityName);
            }
            

            

           


            Console.ReadLine();






            List<string> cool = new List<string>();
            













            //var tradingClient = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET));
            //var stocks = await GetStocks(tradingClient);
            //List<IAsset> TradableStockList = stocks.ToList();
            //string stockNames = "";
            //int c = 0;

            //for (int i = 0, counter = 0; i < TradableStockList.Count; i++, counter++ )
            //{
            //    stockNames += $"{TradableStockList[i].Symbol},";

            //    if (counter == 199)
            //    {
            //        stockNames = stockNames.Remove(stockNames.Length - 1, 1);
            //        var bars = await client.GetBarSetAsync(new BarSetRequest(stockNames, TimeFrame.Minute) { Limit = 1});
            //        string insert = "";
            //        string[] keys = bars.Keys.ToArray();


            //        for (int e = 0; e < bars.Count; e++)
            //        {
            //            for (int g = 0; g < bars[keys[e]].Count; g++)
            //            {
            //                insert += $"({TradableStockList[e].Exchange}),";
            //                c += 1;
            //                Console.WriteLine($"({TradableStockList[e].Exchange}, {TradableStockList[e].Status}, {TradableStockList[e].Symbol}, {c}  ");
            //            }
            //        }

            //        stockNames = "";
            //        counter = 0;
            //    }
            //}

            //Console.WriteLine($"{c}, {TradableStockList.Count}");
            //Console.ReadLine();
            

            //for (int i = 0; i < stocks.Length; i++)
            //{
            //    
            //    string insert = "";
            //    for (int i = 0; i < bars.Count; i++)
            //    {
            //        var listF = tickers.ToList();

            //        for (int e = 0; e < bars[listF[i]].Count; e++)
            //        {
            //            insert += $"({bars[listF[i]][e].Open}, {bars[listF[i]][e].Close}),";
            //        }
            //    }

            //    insert = insert.Remove(insert.Length - 1, 1);

            //    try
            //    {
            //        var cs = "Host=localhost;Username=postgres;Password=password;Database=postgres";

            //        using var con = new NpgsqlConnection(cs);
            //        con.Open();

            //        var sql = $"INSERT INTO test (open, close) VALUES {insert};";

            //        using var cmd = new NpgsqlCommand(sql, con);

            //        var version = cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {

            //        Console.WriteLine(ex.Message);
            //    }
            //}

            

            
            
            
        }

        public static async Task<IEnumerable<IAsset>> GetStocks(IAlpacaTradingClient client)
        {
            var assets = await client.ListAssetsAsync(new AssetsRequest { AssetStatus = AssetStatus.Active });
            return assets.Where(asset => asset.Exchange == Exchange.Nasdaq);
        }  

        //public static string Filter(int index, string info )
        //{
            

        //    List<string> combine = numOne.Concat(numTwo).ToList();

           

            
        //    string returnData = "";

        //    int count = 0;
        //    foreach (var item in stocks)
        //    {
           
        //        Console.WriteLine(item);

        //        if (count == 15)
        //        {
        //            count = 0;
        //        }
        //    }

            
        //    for (int i = 0; i < stocks.Length - 1; i++)
        //    { 
        //        string[] line = stocks[i].Split();

        //        if (line[index] == info)
        //        {
        //            foreach (var item in line)
        //            {
                        
        //                returnData += $"{item},";
        //            }

        //            Console.WriteLine("STOCK FOUND"!);
        //        }

        //    }

        //    return returnData.Remove(returnData.Length - 1, 1);
        //}

        public static List<string> FTPParse(string ftpPath, int fromBack)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string data = streamReader.ReadToEnd();
            string[] lines = data.Split('\n');
            List<string> listLines = lines.ToList();
            listLines.RemoveRange(listLines.Count - fromBack, fromBack);
            return listLines;
        }

        public static IEnumerable<CustomIAsset> GetAssets()
        {
            IEnumerable<CustomIAsset> enumerable = new List<CustomIAsset>();
            List<CustomIAsset> listAssets = enumerable.ToList();
            List<string> numOne = FTPParse("ftp://ftp.nasdaqtrader.com/symboldirectory/nasdaqtraded.txt", 2);
            List<string> numTwo = FTPParse("ftp://ftp.nasdaqtrader.com/symboldirectory/otherlisted.txt", 2);
            for (int i = 1; i < numOne.Count; i++)
            {
                List<string> list = numOne[i].Split('|').ToList();
                CustomIAsset customIAsset = new Asset(list[1], list[2], list[5]);
                listAssets.Add(customIAsset);
            }

            for (int i = 1; i < numTwo.Count; i++)
            {
                List<string> list = numTwo[i].Split('|').ToList();
                CustomIAsset customIAsset = new Asset(list[0], list[1], list[4]);
                listAssets.Add(customIAsset);
            }

            return listAssets.Distinct(IEQ);
        }
    }
}
