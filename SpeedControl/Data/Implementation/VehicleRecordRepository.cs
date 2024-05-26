using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SpeedControl.Data.Interfaces;
using SpeedControl.Models;

namespace SpeedControl.Data.Implementation
{
    public class VehicleRecordRepository : IVehicleRecordRepository
    {
        private readonly string _filePath;

        public VehicleRecordRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Dispose();
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task AddAsync(VehicleRecord vehicleRecord)
        {
            var records = await GetAllAsync() ?? new List<VehicleRecord>();
            var recordsList = new List<VehicleRecord>(records) { vehicleRecord };

            await WriteRecordsToFileAsync(recordsList);
        }

        public async Task<IEnumerable<VehicleRecord>> GetAllAsync()
        {
            List<VehicleRecord>? vehicleRecords;

            if (File.Exists(_filePath))
            {
                using var streamReader = new StreamReader(_filePath);
                using var jsonReader = new JsonTextReader(streamReader);

                var serializer = new JsonSerializer();
                vehicleRecords = serializer.Deserialize<List<VehicleRecord>>(jsonReader);

            }
            else
            {
                throw new FileNotFoundException();
            }

            return vehicleRecords;
        }

        private async Task WriteRecordsToFileAsync(List<VehicleRecord> records)
        {
            using var streamWriter = new StreamWriter(_filePath);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, records);
            await jsonWriter.FlushAsync();
        }
    }
}
