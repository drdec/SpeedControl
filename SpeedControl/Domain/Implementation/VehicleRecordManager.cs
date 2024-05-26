using SpeedControl.Data.Interfaces;
using SpeedControl.Domain.Interfaces;
using SpeedControl.Models;

namespace SpeedControl.Domain.Implementation
{
    public class VehicleRecordManager : IVehicleRecordManager
    {
        private readonly IVehicleRecordRepository _vehicleRecordRepository;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        public VehicleRecordManager(
            IVehicleRecordRepository vehicleRecordRepository,
            DateTime startTime,
            DateTime endTime)
        {
            _vehicleRecordRepository = vehicleRecordRepository;
            _startTime = startTime;
            _endTime = endTime;
        }

        public async Task AddRecordAsync(VehicleRecordRequest record)
        {
            var fullRecord = new VehicleRecord()
            {
                Id = Guid.NewGuid(),
                Number = record.Number,
                Speed = record.Speed,
                Timestamp = record.Timestamp
            };

            await _vehicleRecordRepository.AddAsync(fullRecord);
        }

        public async Task<IEnumerable<VehicleRecord>> GetAllRecordsAsync()
        {
            if (IsValidTime())
            {
                return await _vehicleRecordRepository.GetAllAsync();
            }

            return null;
        }

        public async Task<IEnumerable<VehicleRecord>> GetByDateAndSpeed(DateTime date, float speed)
        {
            if (IsValidTime())
            {
                var records = await _vehicleRecordRepository.GetAllAsync();
                return records.Where(records => records.Timestamp.Date == date.Date && records.Speed > speed);
            }

            return null;
        }

        public async Task<(float, float)> GetMaxAndMinSpeedByDate(DateTime date)
        {
            if (IsValidTime())
            {
                var records = await _vehicleRecordRepository.GetAllAsync();

                var recordsByDate = records.Where(record => record.Timestamp.Date == date);

                if (!recordsByDate.Any())
                {
                    return (0, 0);
                }

                var minSpeed = recordsByDate.Min(rec => rec.Speed);
                var maxSpeed = recordsByDate.Max(rec => rec.Speed);

                return (maxSpeed, minSpeed);
            }

            return (-1, -1);
        }

        private bool IsValidTime() => DateTime.Now >= _startTime && DateTime.Now <= _endTime;
    }
}
