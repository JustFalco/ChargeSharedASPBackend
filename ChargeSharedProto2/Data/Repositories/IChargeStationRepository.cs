using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Data.Repositories;

public interface IChargeStationRepository
{
    Task<IEnumerable<ChargeStation>> GetAllAsync();
    ChargeStation GetChargerById(int id);
    Task<ChargeStation> SaveChargeStationAsync(ChargeStation chargeStation);
}