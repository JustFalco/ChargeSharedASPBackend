using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChargeSharedProto2.Controllers
{
    [Route("api/adress")]
    [ApiController]
    public class AdresController : ControllerBase
    {
        private readonly IAdresService _adresService;

        public AdresController(IAdresService adresService)
        {
            _adresService = adresService;
        }

        // GET: api/<AdresController>
        [HttpGet("{postal}")]
        public async Task<AdressAutoFillDTO> GetAutoFillAdress(string postal)
        {
            return await _adresService.GetAutoFillAdress(postal);
            
        }

    }
}
