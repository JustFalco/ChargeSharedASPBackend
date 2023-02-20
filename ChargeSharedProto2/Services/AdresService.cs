using System.Collections.Immutable;
using ChargeSharedProto2.Data.Repositories;
using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Models;
using Newtonsoft.Json.Linq;

namespace ChargeSharedProto2.Services
{
    public class AdresService : IAdresService
    {
        private readonly IAdresRepository _adresRepository;
        private readonly HttpClient _client;

        /*https://api.mapbox.com/geocoding/v5/mapbox.places/Marktplein%2044-38%2C%207311LR%20Apeldoorn%2C%20Nederland.json?access_token=YOUR_MAPBOX_ACCESS_TOKEN*/
        private string baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
        private string _accessToken = "pk.eyJ1IjoianVzdGZhbGNvIiwiYSI6ImNrZTAwMTh5ZTR4aHQyc3JvdDRxcDl4c3MifQ.Wkvtsel5K0rl3ksLkOt_Yg";
        public AdresService(IAdresRepository adresRepository, HttpClient client)
        {
            this._adresRepository = adresRepository;
            _client = client;
        }

        public async Task<UserAdres> SaveAdresInDatabase(NewAdressDTO newAdress)
        {
            //Pre check if data is conform standard (postal 0000AA etc)

            //Check if adress exists
            if (_adresRepository.CheckIfAdressExists(newAdress.PostalCode, newAdress.HouseNumber, newAdress.HouseAddition, newAdress.Country))
            {
                throw new Exception("Adres already exists in database!");
            }

            //Create new adress object
            UserAdres adres = new UserAdres { 
                City = newAdress.City,
                Street = newAdress.Street,
                Country = newAdress.Country,
                PostalCode = newAdress.PostalCode,
                Province = newAdress.Province,
                HouseAddition = newAdress.HouseAddition,
                HouseNumber = newAdress.HouseNumber,

            };

            //Get Lat and Long from mapbox (features.geometry.coordinates => [])
            string createdUrl = $"{baseUrl}{adres.City}%20{adres.HouseNumber}%20{adres.HouseAddition}%2C%20{adres.PostalCode}%20{adres.City}%2C%20{adres.Country}.json?types=place%2Cpostcode%2Caddress%2Ccountry&access_token={_accessToken}";
            var res = await _client.GetAsync(createdUrl);
            var data = JObject.Parse(await res.Content.ReadAsStringAsync()).SelectToken("features[0].center");
            var center = data.ToArray();
            var lon = center[0].Value<double>();
            var lat = center[1].Value<double>();
            adres.Latitude = lat;
            adres.Longitude = lon;

            //Save new adres in database
            var result = await _adresRepository.SaveAdresAsync(adres);

            //Return result from save
            return result;
        }

        public async Task<AdressAutoFillDTO> GetAutoFillAdress(string postalCode)
        {
            UserAdres? adres = _adresRepository.GetAdresByPostalForAutoFill(postalCode);
            AdressAutoFillDTO adresData = new AdressAutoFillDTO();

            if (adres == null)
            {
                //fetch from mapbox
                string createdUrl =
                    $"{baseUrl}{postalCode}.json?limit=10&types=place%2Cpostcode%2Caddress%2Ccountry&access_token={_accessToken}";
                var res = await _client.GetAsync(createdUrl);
                var contextArr = JObject.Parse(await res.Content.ReadAsStringAsync()).SelectToken("features[0].context")
                    .ToArray();
                foreach (var obJToken in contextArr)
                {
                    var objValue = obJToken.SelectToken("id");
                    var id = objValue.Value<string>();
                    var text = obJToken.SelectToken("text").Value<string>();
                    if (id.Contains("place"))
                    {
                        adresData.City = text;
                    }
                    else if (id.Contains("region"))
                    {
                        adresData.Province = text;
                    }
                    else if (id.Contains("country"))
                    {
                        adresData.Country = text;
                    }
                }

                var data = JObject.Parse(await res.Content.ReadAsStringAsync()).SelectToken("features[0].center");
                var center = data.ToArray();

                var coord1 = center[0].Value<double>();
                var coord2 = center[1].Value<double>();

                string createdUrl2 =
                    $"{baseUrl}{coord1},{coord2}.json?limit=1&types=place%2Cpostcode%2Caddress%2Ccountry&access_token={_accessToken}";
                var res2 = await _client.GetAsync(createdUrl2);

                var textValue = JObject.Parse(await res2.Content.ReadAsStringAsync()).SelectToken("features[0].text").Value<string>();
                adresData.Street = textValue;

                NewAdressDTO newSearchAdres = new NewAdressDTO
                {
                    City = adresData.City,
                    Province = adresData.Province,
                    Country = adresData.Country,
                    HouseAddition = "",
                    HouseNumber = "",
                    PostalCode = postalCode,
                    Street = adresData.Street
                };

                await SaveAdresInDatabase(newSearchAdres);
            }
            else
            {
                adresData.City = adres.City;
                adresData.Country = adres.Country;
                adresData.Province = adres.Province;
                adresData.Street = adres.Street;
            }

            return adresData;
        }
    }
}
