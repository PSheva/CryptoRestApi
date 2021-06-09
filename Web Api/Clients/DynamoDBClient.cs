using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using lectionapi.Extensions;
using Web_Api.Controllers;
using Web_Api.Models;

namespace Web_Api.Clients
{
    public class DynamoDBClient : IDynamoDBClient
    {
        public string _tableName;
        public string _favoritetableName;
        private readonly IAmazonDynamoDB _dynamoDB;
        public DynamoDBClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
            _tableName = Constants.TableName;
            _favoritetableName = Constants.PanicModeDB;
        }


        public async Task<PanicModeDBModel> GetValueByName(string id)
        {

            var item = new GetItemRequest
            {
                TableName = _favoritetableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Currency", new AttributeValue{S=id } }
                }

            };
           

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<PanicModeDBModel>();

            return result;
        }



        public async Task<bool> DeleteElement(string id)
        {
            var item = new DeleteItemRequest
            {
                TableName = _favoritetableName,
                Key = new Dictionary<string, AttributeValue>
                {

                    { "Currency", new AttributeValue { S = id } }
                },
                ReturnValues = "ALL_OLD"

            };

            try
            {
                var response = await _dynamoDB.DeleteItemAsync(item);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                //Console.WriteLine("Here is your error \n" + e);
                return false;
            }
        }

        //public  async Task<bool> DeleteAll(string tableName)
        //{
        // try
        //{
        //    var response = await _dynamoDB.DeleteTableAsync(tableName);

        //    return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        //}
        //catch (Exception e)
        //{

        //    return false;

        //}          

        public async Task<List<CurrencyDBRepository>> GetAllFromDB()
        {
            var result = new List<CurrencyDBRepository>();

            var request = new ScanRequest
            {
                TableName = _tableName,
            };

            var response = await _dynamoDB.ScanAsync(request);
            if (response.Items == null || response.Count == 0)
                return null;

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                result.Add(item.ToClass<CurrencyDBRepository>());
            }
            return result;
        }


        public async Task<bool> PostToFavorite(CurrencyDBRepository data)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Currency", new AttributeValue{S= data.Currency} },
                    {"Genesis_Date", new AttributeValue{S= data.Genesis_Date} },
                    {"Market_Cap_Rank", new AttributeValue{S= data.Market_Cap_Rank} },
                    {"priceUSD", new AttributeValue{S= data.priceUSD} },
                    {"priceUAH", new AttributeValue{S= data.priceUAH} },
                    {"priceRUB", new AttributeValue{S= data.priceRUB} },
                    {"price_change_percentage_24h", new AttributeValue{S= data.price_change_percentage_24h} },
                    {"price_change_percentage_30d", new AttributeValue{S= data.price_change_percentage_30d} },
                    {"price_change_percentage_1y", new AttributeValue{S= data.price_change_percentage_1y} },
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> PanicModePostToDB(PanicModeDBModel panic)
        {
            var request = new PutItemRequest
            {
                TableName = Constants.PanicModeDB,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Currency", new AttributeValue{S= panic.Name} },
                    {"CriticalValue", new AttributeValue{S= panic.CriticalValue} },                    
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }
                   
    }
}
