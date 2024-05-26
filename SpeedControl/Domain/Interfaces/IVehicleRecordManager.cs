using Microsoft.AspNetCore.Routing.Constraints;
using SpeedControl.Models;

namespace SpeedControl.Domain.Interfaces
{
    public interface IVehicleRecordManager
    {
        Task AddRecordAsync(VehicleRecordRequest record);
        Task<IEnumerable<VehicleRecord>> GetAllRecordsAsync();
        Task<(float, float)> GetMaxAndMinSpeedByDate(DateTime date);
        Task<IEnumerable<VehicleRecord>> GetByDateAndSpeed(DateTime date, float speed);
    }
}
