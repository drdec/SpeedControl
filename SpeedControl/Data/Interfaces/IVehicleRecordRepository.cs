using SpeedControl.Models;

namespace SpeedControl.Data.Interfaces
{
    public interface IVehicleRecordRepository
    {
        Task<IEnumerable<VehicleRecord>> GetAllAsync();
        Task AddAsync(VehicleRecord vehicleRecord);
    }
}
