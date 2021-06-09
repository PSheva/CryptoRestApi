using System;
using System.Threading.Tasks;
using static Web_Api.Models.MarketData;

namespace Web_Api.Models
{
    public class CurrencyValue
    {          

        public string Name { get; set; }
        public string Genesis_Date { get; set; }
        public int Market_Cap_Rank { get; set; }
        public MarketData Market_Data { get; set; }
           
          
    }
    public class MarketData
    {
        public CurrentPrice Current_Price{ get; set; }
        public float price_change_percentage_24h { get; set; }
        public float price_change_percentage_7d { get; set; }
        public float price_change_percentage_14d { get; set; }
        public float price_change_percentage_30d { get; set; }
        public float price_change_percentage_60d { get; set; }
        public float price_change_percentage_1y { get; set; }
    }
    public class CurrentPrice
    {
        public float usd { get; set; }
        public float rub { get; set; }
        public float uah { get; set; }
    }

    public class CurrencyValueResponse
    {
        public string Currency{ get; set; }
        public string Genesis_Date { get; set; }
        public string Market_Cap_Rank { get; set; }
        public string priceUSD { get; set; }
        public string priceUAH { get; set; }
        public string priceRUB { get; set; }

        public string price_change_percentage_24h { get; set; }
        //public string price_change_percentage_7d { get; set; }
       // public string price_change_percentage_14d { get; set; }
        public string price_change_percentage_30d { get; set; }
        //public string price_change_percentage_60d { get; set; }
        public string price_change_percentage_1y { get; set; }

        
    }
    

}