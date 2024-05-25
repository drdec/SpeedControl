using SpeedControl.Data.Interfaces;
using SpeedControl.Domain.Interfaces;
using SpeedControl.Models;

namespace SpeedControl.Domain.Implementation
{
    public class VehicleRecordManager : IVehicleRecordManager
    {
        private readonly IVehicleRecordRepository _vehicleRecordRepository;
        
        public VehicleRecordManager(IVehicleRecordRepository vehicleRecordRepository) 
        { 
            _vehicleRecordRepository = vehicleRecordRepository;
        }

        public async Task AddRecordAsync(VehicleRecord record)
        {
            await _vehicleRecordRepository.AddAsync(record);
        }

        public async Task<IEnumerable<VehicleRecord>> GetAllRecordsAsync()
        {
            return await _vehicleRecordRepository.GetAllAsync();
        }

        public async Task<IEnumerable<VehicleRecord>> GetByDateAndSpeed(DateTime date, float speed)
        {
            var records = await _vehicleRecordRepository.GetAllAsync();
            return records.Where(records => records.Timestamp.Date == date.Date && records.Speed > speed);
        }

        public async Task<(float, float)> GetMaxAndMinSpeedByDate(DateTime date)
        {
            var records = await _vehicleRecordRepository.GetAllAsync();

            var recordsByDate = records.Where(record => record.Timestamp.Date == date);

            if (!recordsByDate.Any())
            {
                return (0.0f, 0.0f);
            }

            var minSpeed = recordsByDate.Min(rec => rec.Speed);
            var maxSpeed = recordsByDate.Max(rec => rec.Speed);

            return (maxSpeed, minSpeed);
        }
    }
}
