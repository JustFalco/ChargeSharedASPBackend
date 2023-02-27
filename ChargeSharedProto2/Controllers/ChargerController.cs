using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Data.Repositories;
using ChargeSharedProto2.Models;
using ChargeSharedProto2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChargeSharedProto2.Controllers
{
    [Route("api/chargers")]
    [ApiController]
    public class ChargerController : ControllerBase
    {
        private readonly IChargeStationRepository _repository;
        private readonly IAdresService _adresService;
        private readonly IAdresRepository _adresRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private IEnumerable<IdentityError> _errors { get; set; }

        public ChargerController(IChargeStationRepository repository, IAdresService adresService, IAdresRepository adresRepository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _adresService = adresService;
            _adresRepository = adresRepository;
            _userManager = userManager;
        }

        // GET: api/chargers
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<ChargeStation>> Get()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<ChargerController>/5
        [HttpGet("{id}")]
        public ChargeStation Get(int id)
        {
            return _repository.GetChargerById(id);
        }

        // POST api/<ChargerController>
        /*[Authorize]*/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChargerRegisterDTO charger)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NewAdressDTO newAdress = charger.getAdres();
                    UserAdres adresResult;
                    if (_adresRepository.CheckIfAdressExists(newAdress.PostalCode, newAdress.HouseNumber,
                            newAdress.HouseAddition, newAdress.Country))
                    {
                        adresResult = _adresRepository.GetAdresByDataAsync(newAdress.PostalCode, newAdress.HouseNumber,
                            newAdress.HouseAddition, newAdress.Country);
                    }
                    else
                    {
                        try
                        {
                            adresResult = await _adresService.SaveAdresInDatabase(newAdress);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                    ApplicationUser chargerOwner = await _userManager.FindByEmailAsync(charger.OwnerEmail);
                    
                    ChargeStation newChargeStation = new ChargeStation
                    {
                        ChargerType = (ChargerType)charger.chargerType,
                        Name = charger.chargerName,
                        PricePerHour = charger.pricePerHour,
                        QuickCharge = charger.quickcharge,
                        Adres = adresResult,
                        Owner = chargerOwner
                    };

                    var result = await _repository.SaveChargeStationAsync(newChargeStation);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                foreach (var error in _errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest("Could not save charging station!");
            }
        }

        // PUT api/<ChargerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChargerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _repository.RemoveChargestationById(id);
                return Ok("Charger has been removed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
