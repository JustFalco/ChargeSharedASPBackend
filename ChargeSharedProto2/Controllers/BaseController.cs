using ChargeSharedProto2.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChargeSharedProto2.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly HttpClient _client;

        public BaseController(HttpClient client)
        {
            _client = client;
        }

        // GET: api/<BaseController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BaseController>/5
        [Route("adres")]
        [HttpGet]
        public async Task<string> GetTest(NewAdressDTO adres)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Not valid model state");
            }
            string baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
            string _accessToken = "pk.eyJ1IjoianVzdGZhbGNvIiwiYSI6ImNrZTAwMTh5ZTR4aHQyc3JvdDRxcDl4c3MifQ.Wkvtsel5K0rl3ksLkOt_Yg";
            string createdUrl = $"{baseUrl}{adres.City}%20{adres.HouseNumber}-{adres.HouseAddition}%2C%20{adres.PostalCode}%20{adres.City}%2C%20{adres.Country}.json?access_token={_accessToken}";
            var res = await _client.GetAsync(createdUrl);
            return await res.Content.ReadAsStringAsync();
        }

        // POST api/<BaseController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
