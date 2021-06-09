using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api.Models;

namespace Web_Api.Clients
{
    public interface IDynamoDBClient
    {
        public Task<PanicModeDBModel> GetValueByName(string id);
        public Task<bool> PanicModePostToDB(PanicModeDBModel data);
        public Task<bool> PostToFavorite(CurrencyDBRepository data);
        public Task<List<CurrencyDBRepository>> GetAllFromDB();
        public Task<bool> DeleteElement(string id);
        // public Task<bool> DeleteAll();
        // public Task<bool> PanicModePostToDB(PanicModeDBModel data);
        // public Task<PanicModeDBModel> GetValueByAnyDate(string currency, string date);
        //  public  Task PanicModePostToDB(string currency, string criticalvalue);
        
    }
}
