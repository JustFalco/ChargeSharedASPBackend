using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Services;

public interface IAdresService
{
    Task<UserAdres> SaveAdresInDatabase(NewAdressDTO newAdress);
    Task<AdressAutoFillDTO> GetAutoFillAdress(string postalCode);
}