using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web_Api.Controllers;
using Web_Api.Models;

namespace Web_Api.Clients
{
    
    public class CryptoApiClient
    {
        private HttpClient _client;
        private static string _address;
       

        public CryptoApiClient()
        {
            _address = Constants.address;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }

        public async Task<CurrencyValue> GetValueByName(string id)
        {
            var response = await _client.GetAsync($"{_address}/api/v3/coins/{id}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<CurrencyValue>(content);
            return result;
        }

        public async Task<ValueByDataResponse> GetValueByDate(string id, string date)
        {
            var response = await _client.GetAsync($"{ _address}/api/v3/coins/{id}/history?date={date}");
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<ValueByDataResponse>(content);
            return result;
        }


    }
}
