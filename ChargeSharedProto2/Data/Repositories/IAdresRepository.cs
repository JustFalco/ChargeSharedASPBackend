using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Data.Repositories;

public interface IAdresRepository
{
    Task<IEnumerable<UserAdres>> GetAllAsync();
    bool CheckIfAdressExists(string postalcode, string housnumber, string houseAddition, string country);
    Task<UserAdres> SaveAdresAsync(UserAdres userAdres);

    UserAdres GetAdresByDataAsync(string postalcode, string housnumber, string houseAddition,
        string country);

    UserAdres? GetAdresByPostalForAutoFill(string postalCode);
}