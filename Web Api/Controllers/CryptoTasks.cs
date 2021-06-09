using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using lectionapi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web_Api.Clients;
using Web_Api.Models;


namespace Web_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoTaskController : ControllerBase
    {


        private readonly ILogger<CryptoTaskController> _logger;
        private readonly CryptoApiClient _cryptoApiClient;
        private readonly IDynamoDBClient _dynamoDBClient;
        public CryptoTaskController(ILogger<CryptoTaskController> logger, CryptoApiClient cryptoApiClient, IDynamoDBClient dynamoDBClient)
        {
            _logger = logger;
            _cryptoApiClient = cryptoApiClient;
            _dynamoDBClient = dynamoDBClient;
        }

        [HttpGet("get_data/{id}")]
        public async Task<CurrencyValueResponse> GetData(string id)
        {
            var currencyvalue = await _cryptoApiClient.GetValueByName(id);
            var response = new CurrencyValueResponse
            {
                Currency = currencyvalue.Name,
                Genesis_Date = currencyvalue.Genesis_Date,
                Market_Cap_Rank = currencyvalue.Market_Cap_Rank.ToString(),
                priceUSD = currencyvalue.Market_Data.Current_Price.usd.ToString(),
                priceUAH = currencyvalue.Market_Data.Current_Price.uah.ToString(),
                priceRUB = currencyvalue.Market_Data.Current_Price.rub.ToString(),
                price_change_percentage_24h = currencyvalue.Market_Data.price_change_percentage_24h.ToString(),
                // price_change_percentage_7d = currencyvalue.Market_Data.price_change_percentage_7d.ToString(),
                // price_change_percentage_14d = currencyvalue.Market_Data.price_change_percentage_14d.ToString(),
                price_change_percentage_30d = currencyvalue.Market_Data.price_change_percentage_30d.ToString(),
                //price_change_percentage_60d = currencyvalue.Market_Data.price_change_percentage_60d.ToString(),
                price_change_percentage_1y = currencyvalue.Market_Data.price_change_percentage_1y.ToString()
            };

            return response;

        }

        //[HttpGet("get_data/{id}/history")]
        //public async Task<ValueByDataResponse> GetValueByAnyDate([FromQuery] ValueByDateParemeters paremeters)
        //{
        //    var valuebydata = await _cryptoApiClient.GetValueByDate(paremeters.Currency, paremeters.Date);
        //    return valuebydata;

        //}
        [HttpGet("get_data/{currency}/history/{date}")]
        public async Task<ValueByDataResponse> GetValueByAnyDate(string currency, string date)
        {
            var valuebydata = await _cryptoApiClient.GetValueByDate(currency, date);
            return valuebydata;

        }

        [HttpGet("get_data/getPanic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDataFromDB([FromQuery] string id)
        {
            var result = await _dynamoDBClient.GetValueByName(id);
            if (result == null)
                return NotFound("This record doesn`t exist in database");

            var currencyResponse = new PanicModeDBModel
            {
                Name = id,
                CriticalValue = result.CriticalValue
               
            };
            return Ok(currencyResponse);
        }

        [HttpDelete("delete_fromDB")]
        public async Task<IActionResult> DeleteElement([FromQuery] string id)
        {

            var result = await _dynamoDBClient.DeleteElement(id);

            if (result == false)
            {
                return BadRequest("Your data hasn`t been deleted ");
            }
            return Ok("deleted");

        }
        //[HttpDelete("delete_all_fromDB")]
        //public async Task<IActionResult> DeleteAll()
        //{

        //    var result = await _dynamoDBClient.DeleteAll();

        //    if (result == false)
        //    {
        //        return BadRequest("Your data hasn`t been deleted ");
        //    }
        //    return Ok("deleted");
        //}



        [HttpGet("get_data/get_all_from_DB")]
        public async Task<IActionResult> GetAllFromDB()
        {

            var response = await _dynamoDBClient.GetAllFromDB();
            if (response == null)
                return NotFound("Database is empty");


            var result = response
                .Select(x => new CurrencyDBRepository()
                {
                    Currency = x.Currency,
                    Genesis_Date = x.Genesis_Date,
                    Market_Cap_Rank = x.Market_Cap_Rank,
                    priceUSD = x.priceUSD,
                    priceUAH = x.priceUAH,
                    priceRUB = x.priceRUB,
                    price_change_percentage_24h = x.price_change_percentage_24h,
                    price_change_percentage_30d = x.price_change_percentage_30d,
                    price_change_percentage_1y = x.price_change_percentage_1y
                }).ToList();

            return Ok(result);
        }

       
        [HttpPost("add")]
        public async Task<IActionResult> AddToTrack([FromBody] CurrencyDBRepository response)
        {
            var data = new CurrencyDBRepository
            {
                Currency = response.Currency,
                Genesis_Date = response.Genesis_Date,
                Market_Cap_Rank = response.Market_Cap_Rank,
                priceUSD = response.priceUSD,
                priceUAH = response.priceUAH,
                priceRUB = response.priceRUB,
                price_change_percentage_24h = response.price_change_percentage_24h,
                price_change_percentage_30d = response.price_change_percentage_30d,
                price_change_percentage_1y = response.price_change_percentage_1y,
            };

            var result = await _dynamoDBClient.PostToFavorite(data);

            if (result==false)
            {
                return BadRequest("Your data hasn`t been posted ");
            }
            return Ok("posted");

        }
        //string panic, string criticalvalue
        //[HttpPost("add_to_panic")]
        //public async Task<IActionResult> AddToPanic([FromBody] PanicModeDBModel panic)
        //{
        //    var data = new PanicModeDBModel
        //    {
        //        Name = panic.Name,
        //        CriticalValue = panic.CriticalValue
               
        //    };

        //    var result = await _dynamoDBClient.PanicModePostToDB(data);

        //    if (result == false)
        //    {
        //        return BadRequest("Your data hasn`t been posted ");
        //    }
        //    return Ok("posted");

        //}


        [HttpPost("add_to_panic")]
        public async Task<IActionResult> PostDatatoDynamo([FromBody] PanicModeDBModel panic)
        {
            var data = new PanicModeDBModel
            {
                Name = panic.Name,
                CriticalValue = panic.CriticalValue

            };
            await _dynamoDBClient.PanicModePostToDB(data);
            return Ok();
        }








    }
}
