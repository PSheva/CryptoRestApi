using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api.Models
{
    public class CurrencyDBRepository
    {

        public string Currency { get; set; }
        public string Genesis_Date { get; set; }
        public string Market_Cap_Rank { get; set; }
        public string priceUSD { get; set; }
        public string priceUAH { get; set; }
        public string priceRUB { get; set; }

        public string price_change_percentage_24h { get; set; }
        public string price_change_percentage_30d { get; set; }
        public string price_change_percentage_1y { get; set; }
    }
}
