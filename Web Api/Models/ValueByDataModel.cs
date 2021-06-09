using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api.Models
{
    public class ValueByDataModel
    {
       // public MarketData Market_Data { get; set; }
        public CurrentPrice Current_Price { get; set; }
        public class MarketData
        {
            public CurrentPrice Current_Price { get; set; }
        }
        public class CurrentPrice
        {
            public float Usd { get; set; }
            public float Rub { get; set; }
            public float Uah { get; set; }
        }

    }

    public class ValueByDateParemeters 
    {
    public string Currency { get; set; }
    public string Date { get; set; }
    }


    public class ValueByDataResponse
    {
        public string Name { get; set; }
        public MarketData Market_Data { get; set; }


    }
}
