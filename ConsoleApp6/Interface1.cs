using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp6
{
    interface CustomIAsset
    {
        public string Ticker { get; set; }
        public string SecurityName { get; set; }
        public bool ETF { get; set; }

    }

    public class Asset : CustomIAsset
    {
        public string Ticker { get; set; }
        public string SecurityName { get; set; }
        public bool ETF { get; set; }

        public Asset(string ticker, string securityName, string etf)
        {
            Ticker = ticker;
            SecurityName = securityName;

            if (etf == "Y")
            {
                ETF = true;
            }

            else if (etf == "N")
            {
                ETF = false;

            }
        }
    }
}
