using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
}