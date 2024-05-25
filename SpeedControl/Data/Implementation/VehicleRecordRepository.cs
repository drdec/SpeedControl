using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SpeedControl.Data.Interfaces;
using SpeedControl.Models;

namespace SpeedControl.Data.Implementation
{
    public class VehicleRecordRepository : IVehicleRecordRepository
    {
        private readonly IMemoryCache _cache;
        private readonly string _filePath;
        private const string CacheKey = "VehicleRecordsCache";

        public VehicleRecordRepository(string filePath, IMemoryCache cache)
        {
            _cache = cache;
            _filePath = filePath;
        }

        public async Task AddAsync(VehicleRecord vehicleRecord)
        {
            var records = await GetAllAsync() ?? new List<VehicleRecord>();
            var recordsList = new List<VehicleRecord>(records) { vehicleRecord };

            var json = JsonConvert.SerializeObject(recordsList);
            await File.WriteAllTextAsync(_filePath, json);

            _cache.Set(CacheKey, json);
        }

        public async Task<IEnumerable<VehicleRecord>> GetAllAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out List<VehicleRecord> vehicleRecords))
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    vehicleRecords = JsonConvert.DeserializeObject<List<VehicleRecord>>(json) ?? new List<VehicleRecord>();
                }
                else
                {
                    throw new FileNotFoundException();
                }

                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(40));

                _cache.Set(CacheKey, vehicleRecords, cacheOptions);
            }

            return vehicleRecords;
        }
    }
}
